using UnityEngine;

public static class MapDomain {

    public static MapEntity Spawn(GameContext ctx, int stageID) {
        var map = GameFactory.Map_Create(ctx, stageID);
        ctx.mapRepo.Add(map);
        return map;
    }
}