using UnityEngine;

public static class RoleDomain {

    public static RoleEntity Spawn(GameContext ctx, int typeID, Vector3 pos, Vector3 rotation, Vector3 localScale, Ally ally) {
        var role = Factory.Role_Spawn(ctx, typeID, pos, rotation, localScale, ally);
        ctx.roleRepo.Add(role);
        return role;
    }

    public static void UnSpawn(GameContext ctx) {

    }

    public static void Owner_Move(GameContext ctx, RoleEntity role) {
        role.Move(ctx.input.moveAxis);
        role.Anim_SetSpeed();
    }
}