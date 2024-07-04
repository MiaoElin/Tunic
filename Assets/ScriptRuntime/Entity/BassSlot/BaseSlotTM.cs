using UnityEngine;

[CreateAssetMenu(menuName = "TM/TM_BaseSlot", fileName = "TM_BaseSlot_")]
public class BaseSlotTM : ScriptableObject {
    public int typeID;
    public BaseSlotType baseSlotType;
    public GameObject mod;
}
