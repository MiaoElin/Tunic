using UnityEngine;

public static class GameBusiness_Normal {

    public static void EnterStage(GameContext ctx, int stageID) {
        var owner = RoleDomain.Spawn(ctx, 100, new Vector3(60, 0, 10), Vector3.zero, Vector3.one, Ally.Player);
        ctx.game.ownerID = owner.id;
        owner.weaponType = WeaponType.Sword;
        owner.isOwner = true;

        ctx.camera.SetFollow(owner.transform);
        ctx.camera.SetLookAt(owner.transform);

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
        var input = ctx.input;
        var owner = ctx.GetOwner();
        owner.UpdateInputKey(
        input.isSwordKeyDown,
        input.isShieldKeyDown,
        input.isRangedKeyDown,
        input.isJumpKeyDown,
        input.isInteractKeyDown);
    }

    public static void FixedTick(GameContext ctx, float dt) {

        var owner = ctx.GetOwner();
        RoleFSMController.ApllyFSM(ctx, owner, dt);

        Physics.Simulate(dt);
    }
}
