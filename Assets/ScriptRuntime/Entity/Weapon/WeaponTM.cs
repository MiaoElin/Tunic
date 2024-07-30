using UnityEngine;

[CreateAssetMenu(menuName = "TM/TM_WeaponTM", fileName = "TM_Weapon_")]
public class WeaponTM : ScriptableObject {
    public int typeID;
    public WeaponType weaponType;
    public GameObject mod;
    public int normalSkillTypeID;
    public SkillTM[] skillTMs;
    // public SkillTM[] cancelTMs;
    public Vector3 localPosInRole;
    public Vector3 rotation;
    public string transName;

}