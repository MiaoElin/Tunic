using UnityEngine;
using System.Collections.Generic;

public class SkillComponent {

    Dictionary<InputKeyEnum, SkillSubEntity> all;

    public SkillComponent() {

    }

    public void Ctor() {

    }

    public void Add(SkillSubEntity skill) {
        all.Add(skill.keyEnum, skill);
    }

}