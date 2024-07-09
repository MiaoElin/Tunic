using System;
using UnityEngine;

public static class GameBusiness_Normal {

    public static void EnterStage(GameContext ctx, int stageID) {
        // 生成地图
        var map = MapDomain.Spawn(ctx, stageID);
        // 生成loot
        {
            var lootSpawnerTMs = map.lootSpawnerTMs;
            foreach (var tm in lootSpawnerTMs) {
                LootDomain.Spawn(ctx, tm.lootTypeID, tm.pos, tm.rotation, tm.localScale);
            }
        }

        var owner = RoleDomain.Spawn(ctx, 100, new Vector3(60, 0, 10), Vector3.zero, Vector3.one, Ally.Player);
        ctx.game.ownerID = owner.id;
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

        LateTick(ctx, dt);
    }

    public static void PreTick(GameContext ctx, float dt) {
        var input = ctx.input;
        var owner = ctx.GetOwner();
        owner.UpdateInputKey(
        input.isMeleeKeyDown,
        input.isShieldKeyPress,
        input.isRangedKeyDown,
        input.isJumpKeyDown,
        input.isInteractKeyDown);
    }

    public static void FixedTick(GameContext ctx, float dt) {

        var owner = ctx.GetOwner();
        RoleFSMController.ApllyFSM(ctx, owner, dt);

        ctx.lootRepo.Foreach(loot => {
            LootDomain.HUD_Hints_SHow_Tick(ctx, loot);
        });
        Physics.Simulate(dt);
    }

    private static void LateTick(GameContext ctx, float dt) {
        if (ctx.input.isBagKeyDown) {
            ctx.input.isBagKeyDown = false;
            if (ctx.player.isBagOpen) {
                UIDomain.Panel_Bag_Hide(ctx);
                ctx.player.isBagOpen = false;
                Cursor.lockState = CursorLockMode.Locked;
            } else {
                UIDomain.Panel_Bag_Open(ctx);
                ctx.player.isBagOpen = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
        UIDomain.Panel_Bag_UpdateTick(ctx);
    }
}

