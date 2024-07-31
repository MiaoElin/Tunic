using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "TM/TM_Role", fileName = "TM_Role_")]
public class RoleTM : ScriptableObject {
    public int typeID;
    public AiType aiType;
    public int hpMax;
    public float moveSpeed;
    public float jumpForce;
    public float gravity;
    public int jumpTimesMax;
    public float defense;
    public GameObject mod;
    public List<WeaponTM> weaponTMs;

    public float searchRange;
    public float attackRange;

}