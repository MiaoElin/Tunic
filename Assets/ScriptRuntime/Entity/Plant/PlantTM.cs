using UnityEngine;

[CreateAssetMenu(menuName = "TM/TM_Plant", fileName = "TM_Plant_")]
public class PlantTM : ScriptableObject {
    public int typeID;
    public PlantType plantType;
    public GameObject mod;
}
