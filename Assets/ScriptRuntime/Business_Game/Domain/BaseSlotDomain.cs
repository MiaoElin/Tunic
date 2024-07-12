using UnityEngine;

public static class BaseSlotDomain {

    public static BaseSlotEntity Spawn(GameContext ctx, int typeID, Vector3 pos, Vector3 rotation, Vector3 localScale) {
        var baseslot = GameFactory.BaseSlot_Spawn(ctx, typeID, pos, rotation, localScale);
        ctx.baseSlotRepo.Add(baseslot);
        return baseslot;
    }

    public static void UnSpawn(GameContext ctx, BaseSlotEntity baseSlot) {
        ctx.baseSlotRepo.Remove(baseSlot);
        baseSlot.Reuse();
        ctx.poolService.Return_BaseSlot(baseSlot);
    }
}