using UnityEngine;
using UnityEngine.EventSystems;

public static class Panel_Bag_Domain {

    public static void Open(UIContext ctx) {
        var panel = ctx.uIRepo.TryGet<Panel_Bag>();
        if (panel == null) {
            var name = typeof(Panel_Bag).Name;
            ctx.asset.TryGet_UI_Prefab(name, out var prefab);
            panel = GameObject.Instantiate(prefab, ctx.screenCanvas).GetComponent<Panel_Bag>();
            panel.OnclickGridHandle = (int typeID) => { ctx.eventCenter.Panel_Bag_OnClickGrid(typeID); };
            panel.Ctor();
            ctx.uIRepo.Add(name, panel.gameObject);
            // 设置默认group
            var sword_Btn = panel.GetSword_Btn();
            EventSystem.current.SetSelectedGameObject(sword_Btn.gameObject);
            panel.SetCurrentBtn(sword_Btn);
        }
        panel.gameObject.SetActive(true);
    }

    public static void Update_Tick(UIContext ctx, StuffComponent stuffCom) {
        var panel = ctx.uIRepo.TryGet<Panel_Bag>();
        if (panel == null || panel.gameObject.activeSelf == false) {
            return;
        }
        stuffCom.Foreach(stuff => {
            if (stuff == null) {
                return;
            }
            panel.Init(stuff.index, stuff.typeID, stuff.count, stuff.sprite);
        });
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