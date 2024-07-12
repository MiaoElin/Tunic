using UnityEngine;

[CreateAssetMenu(menuName = "TM/TM_WeaponTM", fileName = "TM_Weapon_")]
public class WeaponTM : ScriptableObject {
    public int typeID;
    public WeaponType weaponType;
    public GameObject mod;
    public SkillTM skillTM;
    public Vector3 localPosInRole;
    public bool isSword;

}