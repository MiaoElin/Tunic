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

    public Transform baseSlotGroup;
    Pool<BaseSlotEntity> baseSlotPool;


    public void Init(Func<RoleEntity> role_Create, Func<WeaponEntity> weapon_Create, Func<LootEntity> loot_Create, Func<BaseSlotEntity> baseSlot_Create) {
        roleGroup = new GameObject("RoleGroup").transform;
        rolePool = new Pool<RoleEntity>(5, role_Create);

        weaponGroup = new GameObject("WeaponGroup").transform;
        weaponPool = new Pool<WeaponEntity>(5, weapon_Create);

        bulletGroup = new GameObject("BulletGroup").transform;

        LootGroup = new GameObject("LootGroup").transform;
        lootPool = new Pool<LootEntity>(5, loot_Create);

        baseSlotGroup = new GameObject("BaseSlotGroup").transform;
        baseSlotPool = new Pool<BaseSlotEntity>(20, baseSlot_Create);
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

    internal BaseSlotEntity Get_BaseSlot() {
        return baseSlotPool.Get();
    }

    public void Return_BaseSlot(BaseSlotEntity slot) {
        baseSlotPool.Return(slot);
    }
}