using UnityEngine;
using System;
using System.Collections.Generic;

public class WeaponEntity : MonoBehaviour {
    public int id;
    public int typeID;
    public Ally ally;
    public WeaponType weaponType;
    public GameObject mod;
    // Skill
    // public SkillComponent skillCom;
    // public int bulletTypeID;//炸弹和子弹都是bullet
    Dictionary<int, SkillSubEntity> skills;
    public int normalSkillTypeID;
    public int currentSkillTypeID;
    public int stuffTypeID;
    public string transName;

    public Action<Collider> OnTriggerEnterHandle;

    public void Ctor(GameObject mod) {
        skills = new Dictionary<int, SkillSubEntity>();
        this.mod = GameObject.Instantiate(mod, transform);
    }

    public void Reuse() {
        Destroy(mod.gameObject);
    }

    internal SkillSubEntity GetCurrentSKill() {
        skills.TryGetValue(currentSkillTypeID, out var skill);
        return skill;
    }

    public void SetCurrentSkillTypeID(int typeID) {
        currentSkillTypeID = typeID;
    }

    public void AddSkill(SkillSubEntity skill) {
        skills.Add(skill.typeID, skill);
    }

    internal void SetLocalPos(Vector3 localPos) {
        transform.localPosition = localPos;
    }

    public void SkillsForeach(Action<SkillSubEntity> action) {
        foreach (var skill in skills) {
            action(skill.Value);
        }
    }
}