using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;
using System;

public class Asset_Core {

    Dictionary<string, GameObject> entity_Prefab;
    AsyncOperationHandle entityPtr;

    Dictionary<string, GameObject> ui_Prefab;
    AsyncOperationHandle uiPtr;

    Dictionary<int, MapTM> mapTMs;
    AsyncOperationHandle mapPtr;

    Dictionary<int, RoleTM> roleTMs;
    AsyncOperationHandle rolePtr;

    Dictionary<int, WeaponTM> weaponTMs;
    AsyncOperationHandle weaponPtr;

    Dictionary<int, LootTM> lootTMs;
    AsyncOperationHandle lootPtr;

    Dictionary<int, StuffTM> stuffTMs;
    AsyncOperationHandle stuffPtr;

    public Asset_Core() {
        entity_Prefab = new Dictionary<string, GameObject>();
        ui_Prefab = new Dictionary<string, GameObject>();
        mapTMs = new Dictionary<int, MapTM>();
        roleTMs = new Dictionary<int, RoleTM>();
        weaponTMs = new Dictionary<int, WeaponTM>();
        lootTMs = new Dictionary<int, LootTM>();
        stuffTMs = new Dictionary<int, StuffTM>();
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
            var ptr = Addressables.LoadAssetsAsync<GameObject>("UI", null);
            uiPtr = ptr;
            var list = ptr.WaitForCompletion();
            foreach (var prefab in list) {
                ui_Prefab.Add(prefab.name, prefab);
            }
        }
        {
            var ptr = Addressables.LoadAssetsAsync<MapTM>("TM_Map", null);
            mapPtr = ptr;
            var list = ptr.WaitForCompletion();
            foreach (var tm in list) {
                mapTMs.Add(tm.stageID, tm);
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
        {
            var ptr = Addressables.LoadAssetsAsync<LootTM>("TM_Loot", null);
            lootPtr = ptr;
            var list = ptr.WaitForCompletion();
            foreach (var tm in list) {
                lootTMs.Add(tm.typeID, tm);
            }
        }
        {
            var ptr = Addressables.LoadAssetsAsync<StuffTM>("TM_Stuff", null);
            stuffPtr = ptr;
            var list = ptr.WaitForCompletion();
            foreach (var tm in list) {
                stuffTMs.Add(tm.typeID, tm);
            }
        }
    }

    public void Unload() {
        Release(entityPtr);
        Release(uiPtr);
        Release(mapPtr);
        Release(rolePtr);
        Release(weaponPtr);
        Release(lootPtr);
        Release(stuffPtr);
    }

    public void Release(AsyncOperationHandle ptr) {
        if (ptr.IsValid()) {
            Addressables.Release(ptr);
        }
    }

    public bool TryGet_Entity_Prefab(string name, out GameObject prefab) {
        return entity_Prefab.TryGetValue(name, out prefab);
    }

    public bool TryGet_UI_Prefab(string name, out GameObject prefab) {
        return ui_Prefab.TryGetValue(name, out prefab);
    }

    public bool TryGet_MapTM(int stageID, out MapTM tm) {
        return mapTMs.TryGetValue(stageID, out tm);
    }

    public bool TryGet_RoleTM(int typeID, out RoleTM tm) {
        return roleTMs.TryGetValue(typeID, out tm);
    }

    public bool TryGet_WeaponTM(int typeID, out WeaponTM tm) {
        return weaponTMs.TryGetValue(typeID, out tm);
    }

    public bool TryGet_LootTM(int typeID, out LootTM tm) {
        return lootTMs.TryGetValue(typeID, out tm);
    }
    public bool TryGet_StuffTM(int typeID, out StuffTM tm) {
        return stuffTMs.TryGetValue(typeID, out tm);
    }
}