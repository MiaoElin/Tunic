using UnityEngine;
using System;

public class PoolService {
    public Transform roleGroup;
    Pool<RoleEntity> rolePool;

    public Transform weaponGroup;
    Pool<WeaponEntity> weaponPool;

    public Transform bulletGroup;


    public void Init(Func<RoleEntity> role_Create, Func<WeaponEntity> weapon_Create) {
        roleGroup = new GameObject("RoleGroup").transform;
        rolePool = new Pool<RoleEntity>(5, role_Create);

        weaponGroup = new GameObject("WeaponGroup").transform;
        weaponPool = new Pool<WeaponEntity>(5, weapon_Create);

        bulletGroup = new GameObject("BulletGroup").transform;
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

}