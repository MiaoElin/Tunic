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

        foreach (var skilltm in tm.skillTMs) {
            SkillSubEntity skill = new SkillSubEntity();
            skill.typeID = skilltm.typeID;
            skill.keyEnum = skilltm.inputKeyEnum;
            skill.anim_Name = skilltm.anim_Name;
            skill.cd = skilltm.cdMax;
            skill.cdMax = skilltm.cdMax;
            skill.precastCDMax = skilltm.precastCDMax;
            skill.castingMaintainSec = skilltm.castingMaintainSec;
            skill.castingIntervalSec = skilltm.castingIntervalSec;
            skill.endCastSec = skilltm.endCastSec;
            role.skillCom.Add_Skill(skill);
        }
        return role;
    }

}