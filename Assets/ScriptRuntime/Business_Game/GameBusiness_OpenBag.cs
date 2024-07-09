using UnityEngine;

public static class GameBusiness_OpenBag {

    public static void Tick(GameContext ctx, float dt) {
        var fsm = ctx.game.fsm;
        if (fsm.isEnterOpenBag) {
            fsm.isEnterOpenBag = false;
            // 打开背包
            UIDomain.Panel_Bag_Open(ctx);
            // 停止游戏时间
            Time.timeScale = 0;
            // 恢复光标
            Cursor.lockState = CursorLockMode.None;
        }
        // 更新背包
        UIDomain.Panel_Bag_UpdateTick(ctx);

        if (ctx.input.isBagKeyDown) {
            // 关闭背包
            ctx.player.isBagOpen = false;
            UIDomain.Panel_Bag_Hide(ctx);
            // 恢复时间
            Time.timeScale = 1;
            // 锁定光标
            Cursor.lockState = CursorLockMode.Locked;

            fsm.EnterNormal();

        }
    }
}