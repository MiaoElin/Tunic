using UnityEngine;

public static class Panel_Bag_Domain {

    public static void Open(UIContext ctx, StuffComponent stuffCom) {
        var panel = ctx.uIRepo.TryGet<Panel_Bag>();
        if (panel == null) {
            var name = typeof(Panel_Bag).Name;
            ctx.asset.TryGet_UI_Prefab(name, out var prefab);
            panel = GameObject.Instantiate(prefab, ctx.screenCanvas).GetComponent<Panel_Bag>();
            panel.Ctor();
            ctx.uIRepo.Add(name, panel.gameObject);
        }
        panel.gameObject.SetActive(true);
    }

    public static void Hide(UIContext ctx) {
        var panel = ctx.uIRepo.TryGet<Panel_Bag>();
        panel?.gameObject.SetActive(false);
    }

    public static void Close(UIContext ctx) {
        var panel = ctx.uIRepo.TryGet<Panel_Bag>();
        if (panel) {
            ctx.uIRepo.Remove(panel.name);
            GameObject.Destroy(panel);
        }
    }
}