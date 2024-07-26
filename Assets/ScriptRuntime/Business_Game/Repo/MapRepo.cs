using UnityEngine;
using System.Collections.Generic;

public class MapRepo {

    Dictionary<int, MapEntity> all;

    public MapRepo() {
        all = new Dictionary<int, MapEntity>();
    }

    public void Add(MapEntity map) {
        all.Add(map.stageID, map);
    }

    public bool TryGet(int stageID, out MapEntity map) {
        return all.TryGetValue(stageID, out map);
    }

    public void Remove(MapEntity map) {
        all.Remove(map.stageID);
    }
}