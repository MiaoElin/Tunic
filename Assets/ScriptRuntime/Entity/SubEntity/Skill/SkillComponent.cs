using UnityEngine;
using System.Collections.Generic;
using System;

public class SkillComponent {

    Dictionary<InputKeyEnum, SkillSubEntity> all;
    public List<InputKeyEnum> usableKeys;
    SkillSubEntity currentSkill;

    public SkillComponent() {
        all = new Dictionary<InputKeyEnum, SkillSubEntity>();
        usableKeys = new List<InputKeyEnum>();
    }

    public void Ctor() {

    }

    public void Add_Skill(SkillSubEntity skill) {
        all.Add(skill.keyEnum, skill);
    }

    public void Add_UsableKeys(InputKeyEnum inputKeyEnum) {
        usableKeys.Add(inputKeyEnum);
    }

    public void Foreach(Action<SkillSubEntity> action) {
        foreach (var skill in all.Values) {
            action.Invoke(skill);
        }
    }

    public void SetCurrentSkill(InputKeyEnum inputKeyEnum) {
        all.TryGetValue(inputKeyEnum, out var skill);
        currentSkill = skill;
    }

    public SkillSubEntity GetCurrentSkill() {
        return currentSkill;
    }
}