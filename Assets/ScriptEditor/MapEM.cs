using UnityEditor;
using UnityEngine;

public class MapEM : MonoBehaviour {
    public MapTM tm;

    [ContextMenu("Save")]
    public void Save() {
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

        EditorUtility.SetDirty(tm);
    }

}