using UnityEngine;
using System.Collections.Generic;

public class ArbitService {
    // key:攻击对象和被攻击对象的类型和id,添加次数
    Dictionary<Bit128, int> all;
    List<Bit128> tempKeysForRemove;

    public ArbitService() {
        all = new Dictionary<Bit128, int>();
        tempKeysForRemove = new List<Bit128>();
    }

    public Bit128 SetKey(EntityType aType, int aID, EntityType bType, int bID) {
        Bit128 key = new Bit128();
        key.i32_0 = (int)aType;
        key.i32_1 = aID;
        key.i32_2 = (int)bType;
        key.i32_3 = bID;
        return key;
    }

    public void Add(EntityType aType, int aID, EntityType bType, int bID) {
        Bit128 key = SetKey(aType, aID, bType, bID);
        if (all.ContainsKey(key)) {
            all[key] += 1;
        } else {
            all.Add(key, 1);
        }
    }

    public bool Has(EntityType aType, int aID, EntityType bType, int bID) {
        Bit128 key = SetKey(aType, aID, bType, bID);
        if (all.ContainsKey(key)) {
            return true;
        } else {
            return false;
        }
    }

    public void RemoveAll(EntityType type, int aID) {
        tempKeysForRemove.Clear();
        foreach (var key in all.Keys) {
            if (key.i32_0 == (int)type && key.i32_1 == aID) {
                tempKeysForRemove.Add(key);
            }
        }
        foreach (var key in tempKeysForRemove) {
            all.Remove(key);
        }
    }
}