using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System;

public class MapEM : MonoBehaviour {
    public MapTM tm;

    // Grid
    [SerializeField] int gridWidth;
    [SerializeField] int gridHeight;
    [SerializeField] float gridSideLength;
    public RectCell3D[] rectCells;

    [ContextMenu("Init")]
    void InitRect() {
        GFpathFinding3D_Rect.Ctor(gridWidth, gridHeight, gridSideLength);

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

    [ContextMenu("Save")]
    public void Save() {
        tm.gridWidth = gridWidth;
        tm.gridHeight = gridHeight;
        tm.gridSideLength = gridSideLength;
        {
            var terrainGroupEM = gameObject.GetComponentInChildren<TerrainGroupEM>();
            tm.terrainTMs = terrainGroupEM.terrainTMs;
        }
        {
            var lootEMs = gameObject.GetComponentsInChildren<LootEM>();
            tm.lootSpawnerTMs = new LootSpawnerTM[lootEMs.Length];
            for (int i = 0; i < lootEMs.Length; i++) {
                var em = lootEMs[i];
                LootSpawnerTM lootSpawnerTM = new LootSpawnerTM() {
                    lootTypeID = em.tm.typeID,
                    pos = em.transform.position,
                    rotation = em.transform.eulerAngles,
                    localScale = em.transform.localScale
                };
                tm.lootSpawnerTMs[i] = lootSpawnerTM;
            }
        }
        {
            var baseSlotEMs = gameObject.GetComponentsInChildren<PlantEM>();
            tm.plantSpawners = new PlantSpawner[baseSlotEMs.Length];
            for (int i = 0; i < baseSlotEMs.Length; i++) {
                var em = baseSlotEMs[i];
                PlantSpawner spawner = new PlantSpawner() {
                    plantTypeID = em.tm.typeID,
                    pos = em.transform.position,
                    rotation = em.transform.eulerAngles,
                    localScale = em.transform.localScale
                };
                tm.plantSpawners[i] = spawner;
            }
        }
        {
            var roleEMs = gameObject.GetComponentsInChildren<RoleEM>();
            tm.roleSpawnerTMs = new RoleSpawnerTM[roleEMs.Length];
            for (int i = 0; i < roleEMs.Length; i++) {
                var em = roleEMs[i];
                RoleSpawnerTM spawner = new RoleSpawnerTM() {
                    roleTypeID = em.tm.typeID,
                    pos = em.transform.position,
                    rotation = em.transform.eulerAngles,
                    localScale = em.transform.localScale
                };
                tm.roleSpawnerTMs[i] = spawner;
            }
        }
        EditorUtility.SetDirty(tm);
    }


    void Update() {
        LayerMask Ground = 1 << 3;
        ref List<Vector2Int> blockSet = ref tm.blockSet;

        if (Input.GetKey(KeyCode.C) && Input.GetKeyDown(KeyCode.Return)) {
            Debug.Log("Clear");
            blockSet.Clear();
            for (int i = 0; i < rectCells.Length; i++) {
                var rect = rectCells[i];
                rect.isBlock = false;
            }
        }

        // 鼠标左键选择格子
        if (Input.GetMouseButton(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool has = Physics.Raycast(ray, out RaycastHit hit, 200, Ground);
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

    private void OnSecneGUI(SceneView sceneView) {
        var current = Event.current;
        switch (current.type) {
            case EventType.KeyUp:
                //键盘按键检测
                break;
            case EventType.MouseUp:
                //鼠标弹起，这里是鼠标所有的点击，如果要在区别如下
                if (current.button == 0) {
                }
                break;
            case EventType.MouseDown:
                //鼠标按下
                break;
            case EventType.MouseDrag:
                //鼠标拖
                break;
            case EventType.Repaint:
                //重绘
                break;
            case EventType.Layout:
                //布局
                break;
        }
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
            if (rect.isBlock) {
                color = Color.red;
            }
            Gizmos.color = color;
            Gizmos.DrawWireCube(rect.worldPos, size);
        }
    }

}