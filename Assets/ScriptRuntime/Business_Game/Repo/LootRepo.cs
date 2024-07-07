using UnityEngine;
using System;
using System.Collections.Generic;

public class LootRepo {
    Dictionary<int, LootEntity> all;
    LootEntity[] temp;

    public LootRepo() {
        all = new Dictionary<int, LootEntity>();
        temp = new LootEntity[128];
    }

    public void Add(LootEntity loot) {
        all.Add(loot.id, loot);
    }

    public void Remove(LootEntity loot) {
        all.Remove(loot.id);
    }

    public void TryGet(int id, out LootEntity loot) {
        all.TryGetValue(id, out loot);
    }

    public int TakeAll(out LootEntity[] allLoot) {
        if (all.Count > temp.Length) {
            temp = new LootEntity[(int)(all.Count * 1.5f)];
        }
        all.Values.CopyTo(temp, 0);
        allLoot = temp;
        return all.Count;
    }

    public void Foreach(Action<LootEntity> action) {
        int LootLen = TakeAll(out var allLoot);
        for (int i = 0; i < LootLen; i++) {
            action.Invoke(allLoot[i]);
        }
    }
}