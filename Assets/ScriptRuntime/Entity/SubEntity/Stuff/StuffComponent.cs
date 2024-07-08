using UnityEngine;
using System;

public class StuffComponent {
    StuffSubEntity[] stuffs;
    int bagGridCount;

    public StuffComponent() {
        bagGridCount = CommonConst.BAG_GROUP_COUNT * CommonConst.BAG_GRIDCOUNT_PERGROUP;
        stuffs = new StuffSubEntity[bagGridCount];
    }

    public void Add(StuffSubEntity stuff, out int leaveCount) {
        leaveCount = 0;
        foreach (var old in stuffs) {
            if (old == null) {
                Debug.Log("Null");
                continue;
            }
            if (old.typeID == stuff.typeID) {
                var usableCount = old.countMax - old.count;
                if (stuff.count <= usableCount) {
                    old.count += stuff.count;
                    return;
                } else {
                    old.count = old.countMax;
                    stuff.count = stuff.count - usableCount;
                    leaveCount = stuff.count;
                }
            }
        }
        Debug.Log(stuff.count);
        // 有剩余
        if (stuff.count > 0) {
            for (int i = 0; i < stuffs.Length; i++) {
                ref var newStuff = ref stuffs[i];
                if (newStuff == null || newStuff.index == -1) {
                    newStuff = stuff;
                    newStuff.index = i;
                    leaveCount = 0;
                    return;
                }
            }
        }
    }

    public void Reuse(int index) {
        var stuff = stuffs[index];
        stuff.index = -1;
        stuff.sprite = null;
        stuff.count = 0;
    }

    public void Foreach(Action<StuffSubEntity> action) {
        foreach (var stuff in stuffs) {
            action.Invoke(stuff);
        }
    }
}