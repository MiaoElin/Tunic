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
    public bool hasDamage;
    public int stuffTypeID;

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

    void OnTriggerEnter(Collider other) {
        Debug.Log("In");
        Debug.Log(hasDamage);
        if (!hasDamage) {
            return;
        }
        OnTriggerEnterHandle.Invoke(other);
    }

    void OnTriggerStay(Collider other) {
        Debug.Log("In");
    }
}