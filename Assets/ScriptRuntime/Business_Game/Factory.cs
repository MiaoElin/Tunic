using UnityEngine;

public static class Factory {

    public static RoleEntity Role_Spawn(GameContext ctx, int typeID, Vector3 pos, Vector3 rotation, Vector3 localScale, Ally ally) {
        bool has = ctx.asset.TryGet_RoleTM(typeID, out var tm);
        if (!has) {
            Debug.Log($"Factory.Role_Spawn {typeID} was not found");
            return null;
        }
        ctx.asset.TryGet_Entity_Prefab(typeof(RoleEntity).Name, out var prefab);
        var role = GameObject.Instantiate(prefab).GetComponent<RoleEntity>();
        role.typeID = typeID;
        role.ally = ally;
        role.SetPos(pos);
        role.SetRotation(rotation);
        role.SetLocalScale(localScale);
        role.Ctor(tm.mod);
        role.id = ctx.iDService.roleIDRecord++;
        role.moveSpeed = tm.moveSpeed;
        return role;
    }
}