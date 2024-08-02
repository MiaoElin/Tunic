using System;
using UnityEngine;

public static class RoleAIFSMController {

    public static void ApplyFSM(GameContext ctx, RoleEntity role, float dt) {
        var status = role.fsm.status;

        ApplyAny(ctx, role, dt);
        if (status == RoleStatus.Normal) {
            ApplyNormal(ctx, role, dt);
        } else if (status == RoleStatus.Casting) {
            ApplyCasting(ctx, role, dt);
        } else if (status == RoleStatus.Suffering) {
            ApplySuffering(ctx, role, dt);
        } else if (status == RoleStatus.Defend) {

        }
    }

    private static void ApplyAny(GameContext ctx, RoleEntity role, float dt) {
        RoleDomain.SkillCD_Tick(role, dt);
        role.aiCom.tree.Execute(dt);
    }

    private static void ApplyNormal(GameContext ctx, RoleEntity role, float dt) {
        var fsm = role.fsm;
        if (fsm.isEnterNormal) {
            fsm.isEnterNormal = false;
        }
        RoleDomain.Falling(role, dt);
    }

    private static void ApplyCasting(GameContext ctx, RoleEntity role, float dt) {
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