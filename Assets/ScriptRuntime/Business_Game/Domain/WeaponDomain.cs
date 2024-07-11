using UnityEngine;

public static class WeaponDomain {

    public static WeaponEntity Spawn(GameContext ctx, int typeID, Transform trans, int stufftypeID, Ally ally) {
        var weapon = Factory.Weapon_Spawn(ctx, typeID, trans, stufftypeID, ally);
        weapon.OnTriggerEnterHandle = (Collider other) => { OnTriggerEnterEvent(ctx, other); };
        return weapon;
    }

    public static void OnTriggerEnterEvent(GameContext ctx, Collider other) {
        if (other.tag == "Grass") {
            BaseSlotEntity grass = other.GetComponentInParent<BaseSlotEntity>();
            GameObject.Destroy(grass.gameObject);
        }
    }
}