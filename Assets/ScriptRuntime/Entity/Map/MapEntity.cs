using UnityEngine;
using System.Collections.Generic;

public class MapEntity : MonoBehaviour {
    public int stageID;
    public TerrainTM[] terrainTMs;
    public LootSpawnerTM[] lootSpawnerTMs;
    public BaseSlotSpawner[] bassSlotSpawners;
    public RoleSpawnerTM[] roleSpawnerTMs;

    // Grid
    public List<Vector2Int> blockSet;
    public RectCell3D[] rectCells;
    public int gridWidth;
    public int gridHeight;
    public float gridSideLength;

}