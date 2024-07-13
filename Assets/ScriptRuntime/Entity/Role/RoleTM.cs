using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "TM/TM_Role", fileName = "TM_Role_")]
public class RoleTM : ScriptableObject {
    public int typeID;
    public AiType aiType;
    public float moveSpeed;
    public float jumpForce;
    public float gravity;
    public int jumpTimesMax;
    public GameObject mod;
    public List<WeaponTM> weaponTMs;

}