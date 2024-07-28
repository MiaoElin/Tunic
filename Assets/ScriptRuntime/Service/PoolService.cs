using UnityEngine;
using System;

public class PoolService {
    public Transform roleGroup;
    Pool<RoleEntity> rolePool;

    public Transform weaponGroup;
    Pool<WeaponEntity> weaponPool;

    public Transform bulletGroup;

    public Transform LootGroup;
    Pool<LootEntity> lootPool;

    public Transform plantGroup;
    Pool<PlantEntity> baseSlotPool;


    public void Init(Func<RoleEntity> role_Create, Func<WeaponEntity> weapon_Create, Func<LootEntity> loot_Create, Func<PlantEntity> plant_Create) {
        roleGroup = new GameObject("RoleGroup").transform;
        rolePool = new Pool<RoleEntity>(5, role_Create);

        weaponGroup = new GameObject("WeaponGroup").transform;
        weaponPool = new Pool<WeaponEntity>(5, weapon_Create);

        bulletGroup = new GameObject("BulletGroup").transform;

        LootGroup = new GameObject("LootGroup").transform;
        lootPool = new Pool<LootEntity>(5, loot_Create);

        plantGroup = new GameObject("PlantGroup").transform;
        baseSlotPool = new Pool<PlantEntity>(20, plant_Create);
    }

    public RoleEntity Get_Role() {
        return rolePool.Get();
    }

    public void Return_Role(RoleEntity role) {
        rolePool.Return(role);
    }

    public WeaponEntity Get_Weapon() {
        return weaponPool.Get();
    }

    public void Return_Weapon(WeaponEntity weapon) {
        weaponPool.Return(weapon);
    }

    public LootEntity Get_Loot() {
        return lootPool.Get();
    }

    public void Return_Loot(LootEntity loot) {
        lootPool.Return(loot);
    }

    internal PlantEntity Get_BaseSlot() {
        return baseSlotPool.Get();
    }

    public void Return_BaseSlot(PlantEntity slot) {
        baseSlotPool.Return(slot);
    }
}