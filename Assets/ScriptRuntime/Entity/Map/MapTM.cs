using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "TM/TM_Map", fileName = "TM_Map_")]
public class MapTM : ScriptableObject {
    public int stageID;
    public TerrainTM[] terrainTMs;
    public LootSpawnerTM[] lootSpawnerTMs;
    public BaseSlotSpawner[] bassSlotSpawners;
    public RoleSpawnerTM[] roleSpawnerTMs;

    // Grid
    public List<Vector2Int> blockSet;
    public int gridWidth;
    public int gridHeight;
    public float gridSideLength;

}