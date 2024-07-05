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

    #region SKill
    public static void SkillCD_Tick(RoleEntity role, float dt) {
        var skilCom = role.skillCom;
        skilCom.Foreach(skill => {
            skill.cd -= dt;
            if (skill.cd <= 0) {
                skill.cd = 0;
            }
        });
    }

    public static bool HasUsableSkill(RoleEntity role) {
        var skillCom = role.skillCom;
        var usableKeys = skillCom.usableKeys;
        usableKeys.Clear();
        skillCom.Foreach(skill => {
            if (skill.cd <= 0) {
                if (skill.keyEnum == InputKeyEnum.Sword) {
                    usableKeys.Add(skill.keyEnum);
                } else if (skill.keyEnum == InputKeyEnum.Ranged) {
                    usableKeys.Add(skill.keyEnum);
                }
            }
        });
        if (usableKeys.Count > 0) {
            return true;
        } else {
            return false;
        }
    }

    // owner 是否按下了发射键
    public static bool HasOwnerCastSkill(RoleEntity role) {
        var skillCom = role.skillCom;
        var currentSkill = skillCom.GetCurrentSkill();
        // 当前有cd<=0的技能
        bool has = HasUsableSkill(role);

        if (!has) {
            return false;
        }

        if (!role.isRangedKeyDown && !role.isSwordKeyDown) {
            // 没按任何技能键
            return false;
        }

        // 判断现在手里的武器
        // 同时按下剑和远攻的时候 如果是剑，那剑优先，如果是远攻，那远攻优先
        // 只按下一个，将当前的武器替换成这个

        if (role.weaponType == WeaponType.None) {
            // 手里没武器
            return false;
        } else if (role.weaponType == WeaponType.Sword) {
            if (role.isSwordKeyDown) {
                skillCom.SetCurrentSkill(InputKeyEnum.Sword);
                role.weaponType = WeaponType.Sword;
            } else if (role.isRangedKeyDown) {
                skillCom.SetCurrentSkill(InputKeyEnum.Ranged);
                role.weaponType = WeaponType.Ranged;
            }
        } else if (role.weaponType == WeaponType.Ranged) {
            if (role.isRangedKeyDown) {
                skillCom.SetCurrentSkill(InputKeyEnum.Ranged);
                role.weaponType = WeaponType.Ranged;
            } else if (role.isSwordKeyDown) {
                skillCom.SetCurrentSkill(InputKeyEnum.Sword);
                role.weaponType = WeaponType.Sword;
            }
        }
        return true;
    }

    public static void Casting(RoleEntity role, float dt) {
        var skilCom = role.skillCom;
        var skill = skilCom.GetCurrentSkill();
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
            }

            if (fsm.castingMaintainTimer <= 0) {
                skillCastStage = SkillCastStage.endCast;
            }
        } else if (skillCastStage == SkillCastStage.endCast) {
            fsm.endCastTimer -= dt;
            if (fsm.endCastTimer <= 0) {
                fsm.isResetCastSkill = true;
                if (role.isOwner) {
                    skilCom.SetCurrentSkill(InputKeyEnum.None);
                }
            }
        }

    }
    #endregion
}