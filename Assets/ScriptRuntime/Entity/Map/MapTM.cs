using UnityEngine;

[CreateAssetMenu(menuName = "TM/TM_Map", fileName = "TM_Map_")]
public class MapTM : ScriptableObject {
    public int stageID;
    public Terrain[] terrains;
    public LootSpawnerTM[] lootSpawnerTMs;
    public BaseSlotSpawner[] bassSlotSpawners;
    public RoleSpawnerTM[] roleSpawnerTMs;
}