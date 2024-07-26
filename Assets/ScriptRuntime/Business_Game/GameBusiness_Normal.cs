using System;
using UnityEngine;

public static class GameBusiness_Normal {

    public static void EnterStage(GameContext ctx, int stageID) {

        // 生成地图
        var map = MapDomain.Spawn(ctx, stageID);

        // 生成Terrain
        foreach (var tm in map.terrainTMs) {
            var terrain = GameObject.Instantiate(tm.mod);
            terrain.transform.position = tm.pos;
        }
        // 生成格子
        var gridWidth = map.gridWidth;
        var gridHeight = map.gridHeight;
        var gridSideLength = map.gridSideLength;
        RectCell3D[] rectCells = new RectCell3D[gridWidth * gridHeight];
        for (int x = 0; x < gridWidth; x++) {
            for (int y = 0; y < gridHeight; y++) {
                RectCell3D rect = new RectCell3D();
                rect.Ctor(x, y, gridSideLength);
                rect.worldPos.y = Terrain.activeTerrain.SampleHeight(rect.worldPos);
                rectCells[y * gridWidth + x] = rect;
            }
        }
        map.rectCells = rectCells;
        // 构造寻路系统
        GFpathFinding3D_Rect.Ctor(map.gridWidth, map.gridHeight, map.gridSideLength);

        // 生成loot
        {
            var lootSpawnerTMs = map.lootSpawnerTMs;
            foreach (var tm in lootSpawnerTMs) {
                LootDomain.Spawn(ctx, tm.lootTypeID, tm.pos, tm.rotation, tm.localScale);
            }
        }

        // 生成baseSlot
        {
            var bassSlotSpawners = map.bassSlotSpawners;
            foreach (var tm in bassSlotSpawners) {
                BaseSlotDomain.Spawn(ctx, tm.bassSlotTypeID, tm.pos, tm.rotation, tm.localScale);
            }
        }

        // 生成Monster
        {
            var roleSpawnerTMs = map.roleSpawnerTMs;
            foreach (var tm in roleSpawnerTMs) {
                RoleDomain.Spawn(ctx, tm.roleTypeID, tm.pos, tm.rotation, tm.localScale, Ally.Monster);
            }
        }

        var owner = RoleDomain.Spawn(ctx, 10, new Vector3(60, 0, 10), Vector3.zero, Vector3.one, Ally.Player);
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

        ctx.roleRepo.Foreach(role => {
            if (role.isOwner) {
                RoleFSMController.ApplyFSM(ctx, owner, dt);
            } else {
                // RoleAIFSMController.ApplyFSM(ctx, role, dt);
                role.aiCom.tree.Execute(dt);
            }
        });

        ctx.lootRepo.Foreach(loot => {
            LootDomain.HUD_Hints_SHow_Tick(ctx, loot);
        });
        Physics.Simulate(dt);

        RoleDomain.Check_Ground(owner);
    }

    private static void LateTick(GameContext ctx, float dt) {
        if (ctx.input.isBagKeyDown) {
            ctx.input.isBagKeyDown = false;
            if (!ctx.player.isBagOpen) {
                ctx.player.isBagOpen = true;
                ctx.game.fsm.EnterOpenBag();
            }
        }
    }
}

