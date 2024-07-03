using UnityEngine;

public static class GameBusiness_Normal {

    public static void EnterStage(GameContext ctx, int stageID) {
        var role = RoleDomain.Spawn(ctx, 100, new Vector3(60, 0, 10), Vector3.zero, Vector3.one, Ally.Player);
        ctx.game.ownerID = role.id;

        ctx.game.fsm.EnterNormal();
    }

    public static void Tick(GameContext ctx, float dt) {

        PreTick(ctx, dt);

        ref float resetTime = ref ctx.resetTime;
        const float INTERVAL = 0.01f;

        resetTime += dt;
        if (resetTime < INTERVAL) {
            FixedTick(ctx, resetTime);
            resetTime = 0;
        } else {
            while (resetTime >= INTERVAL) {
                FixedTick(ctx, INTERVAL);
                resetTime -= INTERVAL;
            }
        }

    }

    public static void PreTick(GameContext ctx, float dt) {

    }
    public static void FixedTick(GameContext ctx, float dt) {

        var owner = ctx.GetOwner();
        RoleDomain.Owner_Move(ctx, owner, dt);

        Physics.Simulate(dt);
    }
}
