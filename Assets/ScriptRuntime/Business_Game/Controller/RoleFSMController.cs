using System;
using UnityEngine;

public static class RoleFSMController {

    public static void ApllyFSM(GameContext ctx, RoleEntity role, float dt) {
        var status = role.fsm.status;

        ApplyAny(role, dt);

        if (status == RoleStatus.Normal) {
            ApllyNormal(ctx, role, dt);
        } else if (status == RoleStatus.Casting) {
            ApllyCasting(ctx, role, dt);
        }
    }

    private static void ApplyAny(RoleEntity role, float dt) {
        RoleDomain.SkillCD_Tick(role, dt);
    }

    private static void ApllyNormal(GameContext ctx, RoleEntity role, float dt) {
        var fsm = role.fsm;
        if (fsm.isEnterNormal) {
            fsm.isEnterNormal = false;
            role.Anim_Idle();
            return;
        }
        // Logic
        RoleDomain.Owner_Move(ctx, role, dt);
        RoleDomain.Defend(role);
        // Exit
        bool has = RoleDomain.HasOwnerCastSkill(role);
        if (has) {
            fsm.EnterCasting();
        }

    }

    private static void ApllyCasting(GameContext ctx, RoleEntity role, float dt) {
        var fsm = role.fsm;
        if (fsm.isEnterCasting) {
            fsm.isEnterCasting = false;
        }

        RoleDomain.Casting(role, dt);

        // Exit
        if (role.skillCom.GetCurrentSkill() == null) {
            fsm.EnterNormal();
        }
    }
}