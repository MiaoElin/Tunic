using System;
using UnityEngine;

public static class RoleFSMController {

    public static void ApplyFSM(GameContext ctx, RoleEntity role, float dt) {
        var status = role.fsm.status;

        ApplyAny(role, dt);
        if (status == RoleStatus.Normal) {
            ApplyNormal(ctx, role, dt);
        } else if (status == RoleStatus.Casting) {
            ApplyCasting(ctx, role, dt);
        } else if (status == RoleStatus.Defend) {
            ApplyDefend(ctx, role, dt);
        } else if (status == RoleStatus.Suffering) {
            ApplySuffering(ctx, role, dt);
        }
    }

    private static void ApplyAny(RoleEntity role, float dt) {
        RoleDomain.SkillCD_Tick(role, dt);
    }

    private static void ApplyNormal(GameContext ctx, RoleEntity role, float dt) {
        var fsm = role.fsm;
        if (fsm.isEnterNormal) {
            fsm.isEnterNormal = false;
            role.Anim_Idle();
            return;
        }
        // Logic
        RoleDomain.Owner_Move(ctx, role, dt);
        RoleDomain.Jump(role);
        RoleDomain.Falling(role, dt);
        RoleDomain.Defend(role);
        RoleDomain.PickLoot(ctx, role);
        RoleDomain.Owner_Focus(ctx, dt);
        // Exit
        bool has = RoleDomain.HasOwnerCastSkill(role);
        if (has) {
            if (role.weaponCom.GetCatingWeapon().weaponType == WeaponType.Shield) {

            } else {
                fsm.EnterCasting();
            }
        }

    }

    private static void ApplyCasting(GameContext ctx, RoleEntity role, float dt) {
        var fsm = role.fsm;
        if (fsm.isEnterCasting) {
            fsm.isEnterCasting = false;
            role.Move_Stop();
        }
        RoleDomain.Casting(ctx, role, dt);
        RoleDomain.Jump(role);
        RoleDomain.Falling(role, dt);
        // Exit
        if (role.weaponCom.GetCatingWeapon() == null) {
            fsm.EnterNormal();
        }
    }

    private static void ApplyDefend(GameContext ctx, RoleEntity role, float dt) {

    }

    private static void ApplySuffering(GameContext ctx, RoleEntity role, float dt) {
        var fsm = role.fsm;
        if (fsm.isEnterSuffering) {
            fsm.isEnterSuffering = false;
        }

        if (fsm.hitBackSec > 0) {
            fsm.hitBackSec -= dt;
        } else {
            fsm.EnterNormal();
        }

        if (fsm.hitUpSec > 0) {
            fsm.hitUpSec -= dt;
        } else {
            fsm.EnterNormal();
        }
        role.Falling(dt);
    }
}