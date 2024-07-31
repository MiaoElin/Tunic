using System;
using UnityEngine;
using System.Linq;

public static class RoleDomain {

    public static RoleEntity Spawn(GameContext ctx, int typeID, Vector3 pos, Vector3 rotation, Vector3 localScale, Ally ally) {
        var role = GameFactory.Role_Spawn(ctx, typeID, pos, rotation, localScale, ally);
        ctx.roleRepo.Add(role);
        role.fsm.EnterNormal();

        // BHTree
        if (role.aiType == AiType.Common) {

            // SearchAction
            BHTreeNode searchAction = new BHTreeNode();
            searchAction.InitAction();
            searchAction.PreconditionHandle = () => {
                if (Vector3.SqrMagnitude(role.Pos() - ctx.GetOwner().Pos()) <= role.searchRange * role.searchRange) {
                    // Debug.Log("HasTarget");
                    return true;
                }
                return false;
            };

            searchAction.ActEnterHandle = (dt) => {
                role.hasTarget = true;
                return BHTreeNodeStatus.Done;
            };

            searchAction.ActNotEnterHandle = (dt) => {
                role.hasTarget = false;
                Vector3 dir = ctx.GetOwner().Pos() - role.Pos();
                role.AI_Move_Stop();
                role.SetForward(dir.normalized, dt);
                return BHTreeNodeStatus.Done;
            };

            // MoveAction
            BHTreeNode moveAction = new BHTreeNode();
            moveAction.InitAction();
            moveAction.PreconditionHandle = () => {
                if (role.hasTarget) {
                    return true;
                } else {
                    return false;
                }
            };


            // moveAction.ActEnterHandle = (dt) => {
            //     return BHTreeNodeStatus.Running;
            // };

            moveAction.ActRunningHandle = (dt) => {
                // Debug.Log("Move");
                // 寻路
                var map = ctx.GetCurrentMap();
                bool has = GFpathFinding3D_Rect.Astar(
                   ctx.GetOwner().Pos(),
                 role.Pos(),
                 (pos) => { return !map.blockSet.Contains(pos); },
                 (index) => { return map.rectCells[index]; },
                 out role.path);
                // Move
                role.MoveBy_Path(dt);
                // Anim
                role.Anim_SetSpeed();
                // 判定是否结束
                Vector3 dir = ctx.GetOwner().Pos() - role.Pos();
                return BHTreeNodeStatus.Running;
            };

            BHTreeNode moveContainer = new BHTreeNode();
            moveContainer.InitContainer(BHTreeNodeType.ParallelOr);
            moveContainer.PreconditionHandle = () => {
                Vector3 dir = ctx.GetOwner().Pos() - role.Pos();
                if (Vector3.SqrMagnitude(dir) <= role.attackRange * role.attackRange) {
                    role.inAttackRange = true;
                    return false;
                } else {
                    return true;
                }
            };

            moveContainer.ActNotEnterHandle = (dt) => {
                Vector3 dir = ctx.GetOwner().Pos() - role.Pos();
                role.AI_Move_Stop();
                role.SetForward(dir.normalized, dt);
                return BHTreeNodeStatus.Done;
            };

            moveContainer.childrens.Add(moveAction);
            // moveContainer.childrens.Add(searchAction);

            // Attack
            BHTreeNode attackAction = new BHTreeNode();
            attackAction.InitAction();
            attackAction.PreconditionHandle = () => {
                if (role.inAttackRange) {
                    if (HasUsableWeapon(role)) {
                        if (!role.fsm.isEnterCasting) {
                            role.fsm.EnterCasting();
                        }
                        return true;
                    }
                }
                return false;
            };

            attackAction.ActRunningHandle = (dt) => {
                AI_SetCastingWeapon(role);
                Casting(role, dt);
                // if (role.GetCastingWeapon() == null) {
                //     role.fsm.EnterNormal();
                // }
                return BHTreeNodeStatus.Running;
            };

            moveContainer.childrens.Add(searchAction);
            moveContainer.childrens.Add(moveAction);

            BHTreeNode root = new BHTreeNode();
            root.InitContainer(BHTreeNodeType.SelectorSequence);
            root.childrens.Add(moveContainer);
            root.childrens.Add(attackAction);


            BHTree tree = new BHTree();
            tree.InitRoot(root);
            role.aiCom.tree = tree;
        }

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
        } else if (role.aiType == AiType.Common) {
            var map = ctx.GetCurrentMap();
            Debug.Log(role.GetVelocityY());
            if (Vector3.SqrMagnitude(role.Pos() - ctx.GetOwner().Pos()) > 4) {
                bool has = GFpathFinding3D_Rect.Astar(
                    ctx.GetOwner().Pos(),
                  role.Pos(),
                  (pos) => { return !map.blockSet.Contains(pos); },
                  (index) => { return map.rectCells[index]; },
                  out role.path);
                role.MoveBy_Path(dt);
                role.Anim_SetSpeed();
            } else {
                role.AI_Move_Stop();
            }
        }
    }
    #endregion

    #region Physics
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

    private static void Weapon_Attack_Check(RoleEntity role, SkillHitBoxModel hitBoxModel) {
        LayerMask layer = 1 << 6;
        var bodyCenter = role.GetBody_Center();
        var size = hitBoxModel.size;
        var center = bodyCenter + role.GetForward() * size.z / 2;
        var halfSize = size / 2;
        var quat = Quaternion.LookRotation(role.GetForward(), Vector3.up);
        var colliders = Physics.OverlapBox(center, halfSize, quat, layer);
        if (colliders.Length > 0) {
            foreach (var other in colliders) {
                if (other.tag == "Grass") {
                    PlantEntity grass = other.GetComponentInParent<PlantEntity>();
                    GameObject.Destroy(grass.gameObject);
                }
            }
        }
#if UNITY_EDITOR
        Debug.Log(center);
        Debug.DrawLine(center, center + role.GetForward() * size.z / 2, Color.red);
        Debug.DrawLine(role.GetBody_Center() + role.body.transform.right * (-size.x / 2), role.GetBody_Center() + role.body.transform.right * size.x / 2, Color.red);
#endif
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
            weapon.SkillsForeach(skill => {
                skill.cd -= dt;
                if (skill.cd <= 0) {
                    skill.cd = 0;
                }
            });
        });
    }

    public static bool HasUsableWeapon(RoleEntity role) {
        var weaponCom = role.weaponCom;
        var usableWeapons = weaponCom.usableWeapons;
        usableWeapons.Clear();
        weaponCom.Foreach(weapon => {
            bool has = false;
            weapon.SkillsForeach(skill => {
                if (skill.cd <= 0) {
                    has = true;
                    return;
                }
            });
            if (has) {
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
            role.comboCount++;
            role.isMeleeKeyDown = false;
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

    public static void AI_SetCastingWeapon(RoleEntity role) {
        var usableWeapons = role.weaponCom.usableWeapons;
        role.SetCatingWeapon(usableWeapons.First().Value);
    }

    public static void Casting(RoleEntity role, float dt) {
        var weapon = role.weaponCom.GetCatingWeapon();
        var skill = weapon.GetCurrentSKill();
        var fsm = role.fsm;
        ref var skillCastStage = ref fsm.skillCastStage;

        if (fsm.isResetCastSkill) {
            fsm.isResetCastSkill = false;
            fsm.ResetCastSkill(skill);
        }

        if (skill.canCombo && skillCastStage != SkillCastStage.endCast) {
            if (role.isMeleeKeyDown) {
                role.isCombo = true;
                role.skillComboMatainSec = 0.5f;
            } else {
                role.skillComboMatainSec -= dt;
                if (role.skillComboMatainSec <= 0) {
                    role.skillComboMatainSec = 0;
                    role.isCombo = false;
                }
            }
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
                Weapon_Attack_Check(role, skill.actionModel.hitBoxModel);
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

            if (role.isCombo) {
                role.nextWeapon = weapon;
                weapon.SetCurrentSkillTypeID(skill.comboSkillTM.typeID);
                fsm.endCastTimer = 0;
            } else {
                role.nextWeapon = null;
                weapon.SetCurrentSkillTypeID(weapon.normalSkillTypeID);
            }

            if (fsm.endCastTimer <= 0) {
                fsm.isResetCastSkill = true;
                role.SetCatingWeapon(role.nextWeapon);
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
            ctx.asset.TryGet_WeaponTM(stuff.weaponTypeID, out var tm);
            var newWeapon = WeaponDomain.Spawn(ctx, typeID, owner.GetWeaponTrans(stuff.weaponType, tm.transName), typeID, owner.ally);
            owner.AddWeapon(newWeapon);
            owner.stuffCom.Remove(typeID);
        }
    }
    #endregion

    #region Focus
    public static void Owner_Focus(GameContext ctx, float dt) {
        var owner = ctx.GetOwner();

        if (owner.hasTarget) {
            // 聚焦Target
            bool hasTarget = ctx.roleRepo.TryGet(owner.targetID, out var monster);
            if (!hasTarget) {
                owner.hasTarget = false;
            } else {
                if (monster.fsm.status != RoleStatus.Dead) {
                    Vector3 dir = monster.Pos() - owner.Pos();
                    if (Vector3.SqrMagnitude(dir) < owner.searchRange * owner.searchRange) {
                        owner.SetForward(dir, dt);
                    }
                } else {
                    owner.hasTarget = false;
                }
            }
        }

        // 如果按下聚焦键、重新寻找聚焦目标
        if (ctx.input.isFocusKeyDown) {
            bool has = FindNearlyMonster(ctx, out var nearlyMonster);
            if (has) {
                owner.hasTarget = true;
                owner.targetID = nearlyMonster.id;
            }
        }
    }

    public static bool FindNearlyMonster(GameContext ctx, out RoleEntity nearlyMonster) {
        var owner = ctx.GetOwner();
        float nearlyDistance = Mathf.Pow(owner.searchRange, 2);
        nearlyMonster = null;
        int roleLen = ctx.roleRepo.TakeAll(out var allRole);
        for (int i = 0; i < roleLen; i++) {
            var role = allRole[i];
            if (role.ally == Ally.Player) {
                continue;
            }
            Vector3 dir = ctx.GetOwner().Pos() - role.Pos();
            float distance = Vector3.SqrMagnitude(dir);
            if (distance < nearlyDistance) {
                nearlyDistance = distance;
                nearlyMonster = role;
            }
        }

        if (nearlyMonster == null) {
            return false;
        } else {
            return true;
        }

    }

    #endregion
}