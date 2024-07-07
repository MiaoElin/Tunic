using UnityEngine;

public static class LootDomain {

    public static LootEntity Spawn(GameContext ctx, int typeID, Vector3 pos, Vector3 rotation, Vector3 localScale) {
        LootEntity loot = Factory.Loot_Spawn(ctx, typeID, pos, rotation, localScale);
        ctx.lootRepo.Add(loot);
        return loot;
    }
}