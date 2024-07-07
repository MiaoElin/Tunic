using UnityEngine;

public static class MapDomain {

    public static MapEntity Spawn(GameContext ctx, int stageID) {
        var map = Factory.Map_Create(ctx, stageID);
        return map;
    }
}