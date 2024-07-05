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

    public static void UsableSkill(RoleEntity role) {
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
    }

    public static bool HasCastSkill(RoleEntity role) {
        var skillCom = role.skillCom;
        var currentSkill = skillCom.GetCurrentSkill();

        UsableSkill(role);

        if (skillCom.usableKeys.Count == 0) {
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
    #endregion
}