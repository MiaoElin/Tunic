using UnityEngine;

public static class RoleDomain {

    public static RoleEntity Spawn(GameContext ctx, int typeID, Vector3 pos, Vector3 rotation, Vector3 localScale, Ally ally) {
        var role = Factory.Role_Spawn(ctx, typeID, pos, rotation, localScale, ally);
        ctx.roleRepo.Add(role);
        role.fsm.EnterNormal();
        return role;
    }

    public static void UnSpawn(GameContext ctx) {

    }

    #region Move
    public static void Owner_Move(GameContext ctx, RoleEntity role, float dt) {
        role.Move(ctx.input.moveAxis, dt);
        role.Anim_SetSpeed();
    }

    #endregion

    #region SKill
    #endregion
}