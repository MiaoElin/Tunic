using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;
using System;

public class Asset_Core {

    Dictionary<string, GameObject> entity_Prefab;
    AsyncOperationHandle entityPtr;

    Dictionary<int, RoleTM> roleTMs;
    AsyncOperationHandle rolePtr;

    Dictionary<int, WeaponTM> weaponTMs;
    AsyncOperationHandle weaponPtr;

    public Asset_Core() {
        entity_Prefab = new Dictionary<string, GameObject>();
        roleTMs = new Dictionary<int, RoleTM>();
        weaponTMs = new Dictionary<int, WeaponTM>();
    }

    public void LoadAll() {
        {
            var ptr = Addressables.LoadAssetsAsync<GameObject>("Entity", null);
            entityPtr = ptr;
            var list = ptr.WaitForCompletion();
            foreach (var prefab in list) {
                entity_Prefab.Add(prefab.name, prefab);
            }
        }
        {
            var ptr = Addressables.LoadAssetsAsync<RoleTM>("TM_Role", null);
            rolePtr = ptr;
            var list = ptr.WaitForCompletion();
            foreach (var tm in list) {
                roleTMs.Add(tm.typeID, tm);
            }
        }
        {
            var ptr = Addressables.LoadAssetsAsync<WeaponTM>("TM_Weapon", null);
            weaponPtr = ptr;
            var list = ptr.WaitForCompletion();
            foreach (var tm in list) {
                weaponTMs.Add(tm.typeID, tm);
            }
        }
    }

    public void Unload() {
        Release(entityPtr);
        Release(rolePtr);
        Release(weaponPtr);
    }

    public void Release(AsyncOperationHandle ptr) {
        if (ptr.IsValid()) {
            Addressables.Release(ptr);
        }
    }

    public bool TryGet_Entity_Prefab(string name, out GameObject prefab) {
        return entity_Prefab.TryGetValue(name, out prefab);
    }

    public bool TryGet_RoleTM(int typeID, out RoleTM tm) {
        return roleTMs.TryGetValue(typeID, out tm);
    }

    public bool TryGet_WeaponTM(int typeID, out WeaponTM tm) {
        return weaponTMs.TryGetValue(typeID, out tm);
    }
}