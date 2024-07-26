using UnityEngine;
using System;

public class WeaponEntity : MonoBehaviour {
    public int id;
    public int typeID;
    public Ally ally;
    public WeaponType weaponType;
    public GameObject mod;
    // Skill
    // public SkillComponent skillCom;
    // public int bulletTypeID;//炸弹和子弹都是bullet
    SkillSubEntity skill;
    public int stuffTypeID;
    public string transName;

    public Action<Collider> OnTriggerEnterHandle;

    public void Ctor(GameObject mod) {
        this.mod = GameObject.Instantiate(mod, transform);
    }

    public void Reuse() {
        Destroy(mod.gameObject);
    }

    internal SkillSubEntity GetSKill() {
        return skill;
    }

    public void SetSkill(SkillSubEntity skill) {
        this.skill = skill;
    }

    internal void SetLocalPos(Vector3 localPos) {
        transform.localPosition = localPos;
    }
}