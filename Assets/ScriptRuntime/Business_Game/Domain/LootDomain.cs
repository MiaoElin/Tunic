using UnityEngine;

public static class LootDomain {

    public static LootEntity Spawn(GameContext ctx, int typeID, Vector3 pos, Vector3 rotation, Vector3 localScale) {
        LootEntity loot = Factory.Loot_Spawn(ctx, typeID, pos, rotation, localScale);
        ctx.lootRepo.Add(loot);
        return loot;
    }

    public static void HUD_Hints_SHow_Tick(GameContext ctx, LootEntity loot) {
        var owner = ctx.GetOwner();
        bool inRange = PureFunction.IsPostInRange(loot.Pos(), owner.Pos(), CommonConst.OWNER_LOOT_SEARCHRANGE);
        if (inRange) {
            UIDomain.HUD_Hints_Open(ctx, loot.id, loot.Pos() + Vector3.up * CommonConst.HUD_Hints_OFFSET);
        } else {
            UIDomain.HUD_Hints_Hide(ctx, loot.id);
        }
    }
}