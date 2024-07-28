using UnityEngine;
using System;
using System.Collections.Generic;

public class BaseSlotRepo {
    Dictionary<int, PlantEntity> all;
    PlantEntity[] temp;

    public BaseSlotRepo() {
        all = new Dictionary<int, PlantEntity>();
        temp = new PlantEntity[128];
    }

    public void Add(PlantEntity baseSlot) {
        all.Add(baseSlot.id, baseSlot);
    }

    public void Remove(PlantEntity baseSlot) {
        all.Remove(baseSlot.id);
    }

    public void TryGet(int id, out PlantEntity baseSlot) {
        all.TryGetValue(id, out baseSlot);
    }

    public int TakeAll(out PlantEntity[] allBaseSlot) {
        if (all.Count > temp.Length) {
            temp = new PlantEntity[(int)(all.Count * 1.5f)];
        }
        all.Values.CopyTo(temp, 0);
        allBaseSlot = temp;
        return all.Count;
    }

    public void Foreach(Action<PlantEntity> action) {
        int baseSlotLen = TakeAll(out var allbaseSlot);
        for (int i = 0; i < baseSlotLen; i++) {
            action.Invoke(allbaseSlot[i]);
        }
    }
}