using UnityEngine;
using System;
using System.Collections.Generic;

public class BaseSlotRepo {
    Dictionary<int, BaseSlotEntity> all;
    BaseSlotEntity[] temp;

    public BaseSlotRepo() {
        all = new Dictionary<int, BaseSlotEntity>();
        temp = new BaseSlotEntity[128];
    }

    public void Add(BaseSlotEntity baseSlot) {
        all.Add(baseSlot.id, baseSlot);
    }

    public void Remove(BaseSlotEntity baseSlot) {
        all.Remove(baseSlot.id);
    }

    public void TryGet(int id, out BaseSlotEntity baseSlot) {
        all.TryGetValue(id, out baseSlot);
    }

    public int TakeAll(out BaseSlotEntity[] allBaseSlot) {
        if (all.Count > temp.Length) {
            temp = new BaseSlotEntity[(int)(all.Count * 1.5f)];
        }
        all.Values.CopyTo(temp, 0);
        allBaseSlot = temp;
        return all.Count;
    }

    public void Foreach(Action<BaseSlotEntity> action) {
        int baseSlotLen = TakeAll(out var allbaseSlot);
        for (int i = 0; i < baseSlotLen; i++) {
            action.Invoke(allbaseSlot[i]);
        }
    }
}