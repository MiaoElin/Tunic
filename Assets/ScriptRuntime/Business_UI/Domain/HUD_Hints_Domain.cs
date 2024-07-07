using System;
using UnityEngine;

public static class HUD_Hints_Domain {

    public static void Open(UIContext ctx, int id, Vector3 pos, Vector3 target) {
        ctx.hUD_HintsRepo.TryGet(id, out var hud);
        if (hud == null) {
            ctx.asset.TryGet_UI_Prefab(typeof(HUD_Hints).Name, out var prefab);
            hud = GameObject.Instantiate(prefab, ctx.hudCanvas).GetComponent<HUD_Hints>();
            hud.id = id;
            hud.SetPos(pos);
            ctx.hUD_HintsRepo.Add(hud);
        }
        hud.Show();
        hud.SetForward(target);
    }

    public static void Hide(UIContext ctx, int id) {
        ctx.hUD_HintsRepo.TryGet(id, out var hud);
        hud?.Hide();
    }

    internal static void Tick(UIContext ctx, Vector3 pos) {
    }

    public static void Close(UIContext ctx, int id) {
        ctx.hUD_HintsRepo.TryGet(id, out var hud);
        if (hud == null) {
            return;
        }
        ctx.hUD_HintsRepo.Remove(hud);
        GameObject.Destroy(hud.gameObject);
    }
}