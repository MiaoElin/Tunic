using UnityEngine;

[CreateAssetMenu(menuName = "TM/TM_WeaponTM", fileName = "TM_Weapon_")]
public class WeaponTM : ScriptableObject {
    public int typeID;
    public WeaponType weaponType;
    public GameObject mod;
    public int bulletTypeID;//炸弹和子弹都是bullet
}