using UnityEngine;
using System.Collections.Generic;
using System;

public class HUD_HintsRepo {

    Dictionary<int, HUD_Hints> all;
    HUD_Hints[] temp;

    public HUD_HintsRepo() {
        all = new Dictionary<int, HUD_Hints>();
        temp = new HUD_Hints[128];
    }

    public void Add(HUD_Hints hud) {
        all.Add(hud.id, hud);
    }

    public void Remove(HUD_Hints hud) {
        all.Remove(hud.id);
    }

    public bool TryGet(int id, out HUD_Hints hud) {
        return all.TryGetValue(id, out hud);
    }
}