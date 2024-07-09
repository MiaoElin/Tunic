using UnityEngine;

[CreateAssetMenu(menuName = "TM/TM_Stuff", fileName = "TM_Stuff_")]
public class StuffTM : ScriptableObject {
    public int typeID;
    public StuffType stuffType;
    public Sprite sprite;
    public int countMax;

    public bool isGetWeapon;
    public WeaponType weaponType;
    public int weaponTypeID;

    public bool isEating;
    public int eatingTypeID;
}