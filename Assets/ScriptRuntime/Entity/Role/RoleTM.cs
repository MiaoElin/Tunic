using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "TM/TM_Role", fileName = "TM_Role_")]
public class RoleTM : ScriptableObject {
    public int typeID;
    public float moveSpeed;
    public GameObject mod;
    public List<SkillTM> skillTMs;

}