using UnityEngine;
using System.Collections.Generic;
using System;
public class RoleRepo {

    Dictionary<int, RoleEntity> all;
    RoleEntity[] temp;

    public RoleRepo() {
        all = new Dictionary<int, RoleEntity>();
        temp = new RoleEntity[128];
    }

    public void Add(RoleEntity role) {
        all.Add(role.id, role);
    }

    public void Remove(RoleEntity role) {
        all.Remove(role.id);
    }

    public bool TryGet(int id, out RoleEntity role) {
        return all.TryGetValue(id, out role);
    }

    public int TakeAll(out RoleEntity[] allRole) {
        if (all.Count > temp.Length) {
            temp = new RoleEntity[(int)(all.Count * 1.5f)];
        }
        all.Values.CopyTo(temp, 0);
        allRole = temp;
        return all.Count;
    }

    public void Foreach(Action<RoleEntity> action) {
        int roleLen = TakeAll(out var allRole);
        for (int i = 0; i < roleLen; i++) {
            action.Invoke(allRole[i]);
        }
    }
}