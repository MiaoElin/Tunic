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
    List<SkillSubEntity> skills;
    public int stuffTypeID;
    public string transName;

    public Action<Collider> OnTriggerEnterHandle;

    public void Ctor(GameObject mod) {
        skills = new List<SkillSubEntity>();
        this.mod = GameObject.Instantiate(mod, transform);
    }

    public void Reuse() {
        Destroy(mod.gameObject);
    }

    internal SkillSubEntity GetSKill(int index) {
        return skills[index - 1];
    }

    public void AddSkill(SkillSubEntity skill) {
        skills.Add(skill);
    }

    internal void SetLocalPos(Vector3 localPos) {
        transform.localPosition = localPos;
    }

    public void SkillsForeach(Action<SkillSubEntity> action) {
        skills.ForEach(action);
    }
}