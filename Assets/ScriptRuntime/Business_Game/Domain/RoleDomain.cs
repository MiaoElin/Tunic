using System;
using UnityEngine;

public static class RoleDomain {

    public static RoleEntity Spawn(GameContext ctx, int typeID, Vector3 pos, Vector3 rotation, Vector3 localScale, Ally ally) {
        var role = GameFactory.Role_Spawn(ctx, typeID, pos, rotation, localScale, ally);
        ctx.roleRepo.Add(role);
        role.fsm.EnterNormal();
        return role;
    }

    public static void UnSpawn(GameContext ctx) {

    }

    #region Jump
    public static void Jump(RoleEntity role) {
        role.Jump();
    }

    public static void Falling(RoleEntity role, float dt) {
        role.Falling(dt);
    }
    #endregion



    #region Move
    public static void Owner_Move(GameContext ctx, RoleEntity role, float dt) {
        role.Move(ctx.input.moveAxis, dt);
        role.Anim_SetSpeed();
        // var weapon = role.weaponCom.GetCurrentWeapon();
        // if (weapon != null) {
        //     Debug.Log("Change");
        //     weapon.transform.localPosition = Vector3.zero;
        // }
    }

    public static void AI_Move(GameContext ctx, RoleEntity role, float dt) {
        var owner = ctx.GetOwner();
        if (role.aiType == AiType.Flyer) {
            role.MoveTo_Target(owner.Pos(), dt);
        }
    }
    #endregion

    #region Check_Ground
    public static void Check_Ground(RoleEntity role) {
        if (role.GetVelocityY() > 0) {
            return;
        }

        LayerMask layer = 1 << LayerMaskConst.GROUND;
        var size = new Vector3(0.5f, 0.1f, 0.5f);
        var quat = Quaternion.LookRotation(role.GetForward(), Vector3.up);
        Collider[] collider = Physics.OverlapBox(role.Pos(), size, quat, layer);
        if (collider.Length > 0) {
            role.ResetJumpTimes();
        }
    }
    #endregion

    public static void Defend(RoleEntity role) {
        if (role.isShieldKeyPress) {
            role.Anim_Defend(true);
        } else {
            role.Anim_Defend(false);
        }
    }

    #region SKill
    public static void SkillCD_Tick(RoleEntity role, float dt) {
        var weaponCom = role.weaponCom;
        weaponCom.Foreach(weapon => {
            var skill = weapon.GetSKill();
            skill.cd -= dt;
            if (skill.cd <= 0) {
                skill.cd = 0;
            }
        });
    }

    public static bool HasUsableWeapon(RoleEntity role) {
        var weaponCom = role.weaponCom;
        var usableWeapons = weaponCom.usableWeapons;
        usableWeapons.Clear();
        weaponCom.Foreach(weapon => {
            var skill = weapon.GetSKill();
            if (skill.cd <= 0) {
                usableWeapons.Add(weapon.weaponType, weapon);
            }
        });
        if (usableWeapons.Count > 0) {
            return true;
        } else {
            return false;
        }
    }

    // owner 是否按下了发射键
    public static bool HasOwnerCastSkill(RoleEntity role) {
        // 当前有cd<=0的技能
        bool has = HasUsableWeapon(role);

        if (!has) {
            return false;
        }

        // 判断现在手里的武器
        // 1.按类型优先级/2.类型一样的看手里的是什么、跟手里有一样的就用手里的、否则替换
        var weaponCom = role.weaponCom;
        var usableWeapons = weaponCom.usableWeapons;
        if (role.isMeleeKeyDown) {
            bool hasThis = usableWeapons.TryGetValue(WeaponType.Melee, out var weapon);
            if (hasThis) {
                role.SetCatingWeapon(weapon);
                return true;
            }
        }
        if (role.isRangedKeyDown) {
            bool hasThis = usableWeapons.TryGetValue(WeaponType.Shooter, out var weapon);
            if (hasThis) {
                role.SetCatingWeapon(weapon);
                return true;
            }
        }
        if (role.isShieldKeyPress) {
            bool hasThis = usableWeapons.TryGetValue(WeaponType.Shield, out var weapon);
            if (hasThis) {
                role.SetCatingWeapon(weapon);
                return true;
            }
        }
        return false;
    }

    public static void Casting(RoleEntity role, float dt) {
        var weapon = role.weaponCom.GetCatingWeapon();
        var skill = weapon.GetSKill();
        var fsm = role.fsm;
        ref var skillCastStage = ref fsm.skillCastStage;

        if (fsm.isResetCastSkill) {
            fsm.isResetCastSkill = false;
            fsm.ResetCastSkill(skill);
        }

        if (skillCastStage == SkillCastStage.PreCast) {
            role.Anim_Attack(skill.anim_Name);
            fsm.precastTimer -= dt;
            if (fsm.precastTimer <= 0) {
                skillCastStage = SkillCastStage.Casting;
                // 重置cd
                skill.cd = skill.cdMax;
            }
        } else if (skillCastStage == SkillCastStage.Casting) {
            fsm.castingMaintainTimer -= dt;
            fsm.castingIntervalTimer -= dt;
            // 近战武器获得伤害力
            if (weapon.weaponType == WeaponType.Melee) {
                if (weapon.isSword) {
                    Weapon_Attack_Check(role);
                }
            }
            if (fsm.castingIntervalTimer <= 0) {
                fsm.castingIntervalTimer = skill.castingIntervalSec;
                // 远程武器发射子弹
            }

            if (fsm.castingMaintainTimer <= 0) {
                skillCastStage = SkillCastStage.endCast;
            }
        } else if (skillCastStage == SkillCastStage.endCast) {
            fsm.endCastTimer -= dt;
            if (fsm.endCastTimer <= 0) {
                fsm.isResetCastSkill = true;
                if (role.isOwner) {
                    role.SetCatingWeapon(null);
                }
            }
        }

    }

    private static void Weapon_Attack_Check(RoleEntity role) {
        LayerMask layer = 1 << 6;
        var center = role.GetBody_Center() + role.GetForward() * 1;
        var halfSize = new Vector3(0.5f, 1, 1);
        var quat = Quaternion.LookRotation(role.GetForward(), Vector3.up);
        var colliders = Physics.OverlapBox(center, halfSize, quat, layer);
        if (colliders.Length > 0) {
            foreach (var other in colliders) {
                if (other.tag == "Grass") {
                    BaseSlotEntity grass = other.GetComponentInParent<BaseSlotEntity>();
                    GameObject.Destroy(grass.gameObject);
                }
            }
        }
    }
    #endregion

    #region PickLoot
    public static bool FindNearlyLoot(GameContext ctx, RoleEntity owner, out LootEntity loot) {
        float nearlyDistance = Mathf.Pow(CommonConst.OWNER_LOOT_SEARCHRANGE, 2);
        LootEntity nearlyloot = null;
        ctx.lootRepo.Foreach(loot => {
            float distance = Vector3.SqrMagnitude(loot.Pos() - owner.Pos());
            if (distance <= nearlyDistance) {
                nearlyDistance = distance;
                nearlyloot = loot;
            }
        });
        loot = nearlyloot;
        if (nearlyloot = null) {
            return false;
        } else {
            return true;
        }
    }

    public static void PickLoot(GameContext ctx, RoleEntity owner) {
        bool has = FindNearlyLoot(ctx, owner, out var nearlyLoot);
        if (has) {
            if (owner.isInteractKeyDown) {
                owner.isInteractKeyDown = false; // 这里不设false为什么会执行两次
                // 生成stuff添加进背包里

                var typeCount = nearlyLoot.stufftypeIDs.Length;
                var stufftypeIDs = nearlyLoot.stufftypeIDs;
                var stuffCounts = nearlyLoot.stuffCounts;
                for (int i = 0; i < typeCount; i++) {
                    var typeID = stufftypeIDs[i];
                    var count = stuffCounts[i];
                    var stuff = GameFactory.Stuff_Create(ctx, typeID, count);
                    owner.stuffCom.Add(stuff, out var leaveCount);
                }

                // 销毁loot/HUD_Close
                LootDomain.Unspawn(ctx, nearlyLoot);
                UIDomain.HUD_Hints_Close(ctx, nearlyLoot.id);
            }
        }
    }
    #endregion

    #region Stuff
    public static void Owner_UseStuff(GameContext ctx, int typeID) {
        var owner = ctx.GetOwner();
        bool has = owner.stuffCom.TryGet(typeID, out var stuff);
        if (!has) {
            Debug.LogError($"{typeID} was not found");
        }

        if (stuff.isGetWeapon) {
            bool hasThisType = owner.weaponCom.TryGet(stuff.weaponType, out var weapon);
            if (hasThisType) {
                // 从准备区移除
                owner.weaponCom.Remove(weapon);
                // 返回背包
                var backStuff = GameFactory.Stuff_Create(ctx, weapon.stuffTypeID, 1);
                owner.stuffCom.Add(backStuff, out var leaveCount);
                if (leaveCount > 0) {
                    Debug.Log("背包满了");
                }
                GameObject.Destroy(weapon.gameObject);
            }
            var newWeapon = WeaponDomain.Spawn(ctx, typeID, owner.GetWeaponTrans(stuff.weaponType), typeID, owner.ally);
            owner.AddWeapon(newWeapon);
            owner.stuffCom.Remove(typeID);
        }
    }
    #endregion
}