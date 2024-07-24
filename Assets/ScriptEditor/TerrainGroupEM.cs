using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[ExecuteInEditMode]
public class TerrainGroupEM : MonoBehaviour {

    // Terrain
    [SerializeField] TerrainTM[] terrainTMs;
    [SerializeField] float terrainSingle_Width;
    [SerializeField] float terrainSingle_Height;
    [SerializeField] int terrainWidth; // Count
    [SerializeField] int terrainHeight; // Count
    public GameObject[] terrains;

    // Grid
    [SerializeField] int gridWidth;
    [SerializeField] int gridHeight;
    [SerializeField] float gridSideLength;
    public RectCell3D[] rectCells;
    List<Vector2Int> blockSet = new List<Vector2Int>();
    Vector3[] arround = new Vector3[4];

    [ContextMenu("Init")]
    void Awake() {
        GFpathFinding3D_Rect.Ctor(gridWidth, gridHeight, gridSideLength);
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
                    int index = y * gridWidth + x;
                    ref var terrainTM = ref terrainTMs[index];
                    terrainTM.pos = new Vector3(terrainSingle_Width * x, 0, terrainSingle_Height * y);
                    GameObject mod = GameObject.Instantiate(terrainTM.mod, transform);
                    mod.layer = 3;
                    terrains[index] = mod;
                }
            }
        }

        {
            // 生成格子
            rectCells = new RectCell3D[gridWidth * gridHeight];
            for (int x = 0; x < gridWidth; x++) {
                for (int y = 0; y < gridHeight; y++) {
                    RectCell3D rect = new RectCell3D();
                    rect.Ctor(x, y, gridSideLength);
                    rect.worldPos.y = Terrain.activeTerrain.SampleHeight(rect.worldPos);
                    rectCells[y * gridWidth + x] = rect;
                }
            }
        }
    }

    void Update() {
        LayerMask Ground = 1 << 3;
        // 鼠标左键选择格子
        if (Input.GetMouseButton(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool has = Physics.Raycast(ray, out RaycastHit hit, 200, Ground);
            Debug.Log(has);
            if (has) {
                var gridPos = GFpathFinding3D_Rect.WorldToGridPos(hit.point);
                var index = GFpathFinding3D_Rect.GetIndex(gridPos);
                ref RectCell3D cell = ref rectCells[index];
                if (cell.isBlock) {
                    if (Input.GetKey(KeyCode.C)) {
                        cell.isBlock = false;
                        blockSet.Remove(cell.pos);
                    }
                } else if (!Input.GetKey(KeyCode.C)) {
                    cell.isBlock = true;
                    blockSet.Add(cell.pos);
                }
            }
        }
    }

    void OnGUI() {
        // if (rectCells == null) {
        //     return;
        // }
        // foreach (var rect in rectCells) {
        //     var screenPos = Camera.main.WorldToScreenPoint(rect.worldPos);
        //     GUI.Label(new Rect(screenPos.x - 20, Screen.height - screenPos.y, 40, 20), $"({rect.pos.x},{rect.pos.y})");
        // }
    }

    void OnDrawGizmos() {
        if (rectCells == null) {
            return;
        }
        if (UnityEditor.Selection.activeGameObject != gameObject) {
            return;
        }
        float sideLen = gridSideLength;
        // Vector3 offset = new Vector3(sideLen * 0.5f, 0, sideLen * 0.5f);
        Vector3 size = new Vector3(sideLen, 0.0f, sideLen);
        Span<Vector3> sides = stackalloc Vector3[4];
        foreach (var rect in rectCells) {
            Color color = Color.blue;
            if (rect.isClose) {
                color = Color.black;
            }
            if (rect.isBlock) {
                color = Color.red;
            }
            Gizmos.color = color;
            Gizmos.DrawWireCube(rect.worldPos, size);
        }
    }
}