using UnityEngine;

public static class Factory {
    #region Role
    public static RoleEntity Role_Create(GameContext ctx) {
        ctx.asset.TryGet_Entity_Prefab(typeof(RoleEntity).Name, out var prefab);
        RoleEntity role = GameObject.Instantiate(prefab, ctx.poolService.roleGroup).GetComponent<RoleEntity>();
        role.gameObject.SetActive(false);
        return role;
    }

    public static RoleEntity Role_Spawn(GameContext ctx, int typeID, Vector3 pos, Vector3 rotation, Vector3 localScale, Ally ally) {
        bool has = ctx.asset.TryGet_RoleTM(typeID, out var tm);
        if (!has) {
            Debug.Log($"Factory.Role_Spawn {typeID} was not found");
            return null;
        }

        RoleEntity role = ctx.poolService.Get_Role();
        role.typeID = typeID;
        role.ally = ally;
        role.SetPos(pos);
        role.SetRotation(rotation);
        role.SetLocalScale(localScale);
        role.Ctor(tm.mod);
        role.id = ctx.iDService.roleIDRecord++;
        role.moveSpeed = tm.moveSpeed;
        if (tm.weaponTMs != null) {
            foreach (var weaponTM in tm.weaponTMs) {
                WeaponEntity weapon = Weapon_Spawn(ctx, weaponTM.typeID, role.GetWeaponTrans(weaponTM.weaponType));
                weapon.transform.localPosition = Vector3.zero;
                weapon.transform.localEulerAngles = Vector3.zero;
                weapon.transform.localScale = Vector3.one;
                role.weaponCom.Add(weapon);
            }
        }
        role.gameObject.SetActive(true);
        return role;
    }
    #endregion

    #region Weapon
    public static WeaponEntity Weapon_Create(GameContext ctx) {
        ctx.asset.TryGet_Entity_Prefab(typeof(WeaponEntity).Name, out var prefab);
        WeaponEntity weapon = GameObject.Instantiate(prefab, ctx.poolService.roleGroup).GetComponent<WeaponEntity>();
        weapon.gameObject.SetActive(false);
        return weapon;
    }

    public static WeaponEntity Weapon_Spawn(GameContext ctx, int typeID, Transform weaponTrans) {
        ctx.asset.TryGet_WeaponTM(typeID, out var tm);
        if (!tm) {
            Debug.LogError($"Factory.Weapon_Spawn {typeID} was not Found");
        }

        WeaponEntity weapon = ctx.poolService.Get_Weapon();
        weapon.transform.SetParent(weaponTrans);
        weapon.Ctor(tm.mod);
        weapon.typeID = tm.typeID;
        weapon.id = ctx.iDService.weaponRecord++;
        {
            SkillSubEntity skill = new SkillSubEntity();
            var skilltm = tm.skillTM;
            skill.typeID = skilltm.typeID;
            skill.keyEnum = skilltm.inputKeyEnum;
            skill.anim_Name = skilltm.anim_Name;
            skill.cd = skilltm.cdMax;
            skill.cdMax = skilltm.cdMax;
            skill.precastCDMax = skilltm.precastCDMax;
            skill.castingMaintainSec = skilltm.castingMaintainSec;
            skill.castingIntervalSec = skilltm.castingIntervalSec;
            skill.endCastSec = skilltm.endCastSec;
            weapon.SetSkill(skill);
        }
        weapon.gameObject.SetActive(true);
        return weapon;
    }
    #endregion

    #region Loot
    public static LootEntity Loot_Create(GameContext ctx) {
        ctx.asset.TryGet_Entity_Prefab(typeof(LootEntity).Name, out var prefab);
        LootEntity loot = GameObject.Instantiate(prefab, ctx.poolService.LootGroup).GetComponent<LootEntity>();
        loot.gameObject.SetActive(false);
        return loot;
    }

    public static LootEntity Loot_Spawn(GameContext ctx, int typeID, Vector3 pos, Vector3 rotation, Vector3 localScale) {
        ctx.asset.TryGet_LootTM(typeID, out var tm);
        if (!tm) {
            Debug.LogError($"Factory.Loot_Spawn {typeID} was not found");
        }

        LootEntity loot = ctx.poolService.Get_Loot();

        loot.typeID = typeID;
        loot.SetPos(pos);
        loot.SetRotaion(rotation);
        loot.SetLocalScale(localScale);
        loot.Ctor(tm.mod);
        loot.id = ctx.iDService.lootIDRecord++;
        loot.stufftypeIDs = tm.stufftypeIDs;
        loot.stuffCount = tm.stuffCount;
        loot.gameObject.SetActive(true);
        return loot;
    }
    #endregion

    #region Map
    public static MapEntity Map_Create(GameContext ctx, int stageID) {
        ctx.asset.TryGet_MapTM(stageID, out var tm);
        ctx.asset.TryGet_Entity_Prefab(typeof(MapEntity).Name, out var prefab);
        MapEntity map = GameObject.Instantiate(prefab).GetComponent<MapEntity>();

        map.stageID = stageID;
        // 后面补terrain
        // map.terrains = new Terrain[tm.terrains.Length];
        // for (int i = 0; i < tm.terrains.Length; i++) {
        //     var terrain = map.terrains[i];
        //     terrain = GameObject.Instantiate(tm.terrains[i], map.transform).GetComponent<Terrain>();
        //     // terrain.transform.position= girdpos*terrainSize;
        //     map.terrains[i]=terrain;
        // }
        map.lootSpawnerTMs = tm.lootSpawnerTMs;
        map.bassSlotSpawners = tm.bassSlotSpawners;
        map.roleSpawnerTMs = tm.roleSpawnerTMs;
        return map;
    }
    #endregion

    #region Stuff
    // public static StuffSubEntity Stuff_Create(GameContext ctx, int typeID) {
    //     ctx.asset.
    // }
    #endregion
}