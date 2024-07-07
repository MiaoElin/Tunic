using UnityEngine;

public static class UIDomain {

    public static void HUD_Hints_Open(GameContext ctx, int id, Vector3 pos) {
        ctx.uIApp.HUD_Hints_Open(id, pos, ctx.camera.Pos());
    }

    public static void HUD_Hints_Hide(GameContext ctx, int id) {
        ctx.uIApp.HUD_Hints_Hide(id);
    }

    public static void HUD_Hints_Close(GameContext ctx, int id) {
        ctx.uIApp.HUD_Hints_Close(id);
    }

    public static void HUD_Hints_Tick(GameContext ctx) {
        ctx.uIApp.HUD_Hints_Tick(ctx.camera.Pos());
    }

}