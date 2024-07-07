using UnityEngine;

public static class RoleDomain {

    public static RoleEntity Spawn(GameContext ctx, int typeID, Vector3 pos, Vector3 rotation, Vector3 localScale, Ally ally) {
        var role = Factory.Role_Spawn(ctx, typeID, pos, rotation, localScale, ally);
        ctx.roleRepo.Add(role);
        role.fsm.EnterNormal();
        return role;
    }

    public static void UnSpawn(GameContext ctx) {

    }

    #region Move
    public static void Owner_Move(GameContext ctx, RoleEntity role, float dt) {
        role.Move(ctx.input.moveAxis, dt);
        role.Anim_SetSpeed();
    }

    #endregion

    public static void Defend(RoleEntity role) {
        if (role.isShieldKeyPress) {
            role.Anim_Defend();
        } else {
            role.anim.ResetTrigger("T_Defend");
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
                weaponCom.SetCurrentWeapon(weapon);
                return true;
            }
        }
        if (role.isRangedKeyDown) {
            bool hasThis = usableWeapons.TryGetValue(WeaponType.Shooter, out var weapon);
            if (hasThis) {
                weaponCom.SetCurrentWeapon(weapon);
                return true;
            }
        }
        if (role.isShieldKeyPress) {
            bool hasThis = usableWeapons.TryGetValue(WeaponType.Shield, out var weapon);
            if (hasThis) {
                weaponCom.SetCurrentWeapon(weapon);
                return true;
            }
        }
        return false;
    }

    public static void Casting(RoleEntity role, float dt) {
        var weapon = role.weaponCom.GetCurrentWeapon();
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
                    role.weaponCom.SetCurrentWeapon(null);
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
                Debug.Log(Time.frameCount + "pick" + nearlyLoot.id);
                // 生成stuff添加进背包里
                // 销毁loot/HUD_Close
            }
        }
    }


    #endregion
}