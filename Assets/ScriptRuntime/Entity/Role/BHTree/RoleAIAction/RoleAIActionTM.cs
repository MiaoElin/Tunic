using UnityEngine;
using System;

[Serializable]
public class RoleAIActionTM {
    public int typeID;
    // Search
    public bool isSearchTarget;
    public bool isSearchPlayerOwner;
    public float searchRange;

    // Move 
    public bool isMoveToTarget;

    // Attack
    public bool isCastingSkill;
}