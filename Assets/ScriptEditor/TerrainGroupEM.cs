using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[ExecuteInEditMode]
public class TerrainGroupEM : MonoBehaviour {

    // Terrain
    [SerializeField] public TerrainTM[] terrainTMs;
    [SerializeField] float terrainSingle_Width;
    [SerializeField] float terrainSingle_Height;
    [SerializeField] int terrainWidth; // Count
    [SerializeField] int terrainHeight; // Count
    public GameObject[] terrains;

    [ContextMenu("Init")]
    void Awake() {
        {
            // 生成Terrains
            if (terrains != null) {
                foreach (var terrain in terrains) {
                    DestroyImmediate(terrain.gameObject);
                }
            }
            terrains = new GameObject[terrainTMs.Length];
            for (int x = 0; x < terrainWidth; x++) {
                for (int y = 0; y < terrainHeight; y++) {
                    int index = y * terrainWidth + x;
                    ref var terrainTM = ref terrainTMs[index];
                    terrainTM.pos = new Vector3(terrainSingle_Width * x, 0, terrainSingle_Height * y);
                    GameObject mod = GameObject.Instantiate(terrainTM.mod, transform);
                    mod.layer = 3;
                    terrains[index] = mod;
                }
            }
        }

    }

}