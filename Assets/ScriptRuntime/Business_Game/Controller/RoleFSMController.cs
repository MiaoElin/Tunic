using System;
using UnityEngine;

public static class RoleFSMController {

    public static void ApllyFSM(GameContext ctx, RoleEntity role, float dt) {
        var status = role.fsm.status;
        if (status == RoleStatus.Normal) {
            ApllyNormal(ctx, role, dt);
        }
    }

    private static void ApllyNormal(GameContext ctx, RoleEntity role, float dt) {
        var fsm = role.fsm;
        if (fsm.isEnterNormal) {
            fsm.isEnterNormal = false;
        }
        RoleDomain.Owner_Move(ctx, role, dt);
    }
}