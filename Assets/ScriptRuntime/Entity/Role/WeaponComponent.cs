using UnityEngine;
using System.Collections.Generic;
using System;

public class WeaponComponent {

    Dictionary<WeaponType, WeaponEntity> ready;
    public Dictionary<WeaponType, WeaponEntity> usableWeapons;
    WeaponEntity castingWeapon;

    public WeaponComponent() {
        ready = new Dictionary<WeaponType, WeaponEntity>();
        usableWeapons = new Dictionary<WeaponType, WeaponEntity>();
    }

    public void Add(WeaponEntity weapon) {
        bool has = ready.TryGetValue(weapon.weaponType, out var weaponIN);
        if (has) {
            Remove(weaponIN);
            GameObject.Destroy(weaponIN.gameObject);
        }
        ready.Add(weapon.weaponType, weapon);
        Debug.Log(ready.Count);
    }

    public void Remove(WeaponEntity weapon) {
        ready.Remove(weapon.weaponType);
    }

    public void Foreach(Action<WeaponEntity> action) {
        foreach (var weapon in ready) {
            action(weapon.Value);
        }
    }

    public void SetCurrentWeapon(WeaponEntity weapon) {
        castingWeapon = weapon;
    }

    public WeaponEntity GetCurrentWeapon() {
        // all.TryGetValue(currentWeaponID, out var weapon);
        // return weapon;
        return castingWeapon;
    }


}