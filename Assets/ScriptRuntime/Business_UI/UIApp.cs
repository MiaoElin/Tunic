using System;
using UnityEngine;

public class UIApp {
    UIContext ctx;

    public UIApp() {
        ctx = new UIContext();
    }

    public void Inject(Canvas hudCanvas, Canvas screenCanvas, Asset_Core asset) {
        ctx.Inject(hudCanvas, screenCanvas, asset);
    }

    public void HUD_Hints_Open(int id, Vector3 pos, Vector3 target) {
        HUD_Hints_Domain.Open(ctx, id, pos, target);
    }

    public void HUD_Hints_Hide(int id) {
        HUD_Hints_Domain.Hide(ctx, id);
    }

    public void HUD_Hints_Close(int id) {
        HUD_Hints_Domain.Close(ctx, id);
    }

    internal void HUD_Hints_Tick(Vector3 pos) {
        HUD_Hints_Domain.Tick(ctx, pos);
    }

    public void Panel_Bag_Open() {
        Panel_Bag_Domain.Open(ctx);
    }

    public void Panel_Bag_UpdateTick(StuffComponent stuffCom) {
        Panel_Bag_Domain.Update_Tick(ctx, stuffCom);
    }

    public void Panel_Bag_Hide() {
        Panel_Bag_Domain.Hide(ctx);
    }

    public void Panel_Bag_Close() {
        Panel_Bag_Domain.Close(ctx);
    }

}