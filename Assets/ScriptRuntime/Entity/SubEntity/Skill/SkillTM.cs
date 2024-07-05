using UnityEngine;

[CreateAssetMenu(menuName = "TM/TM_Skill", fileName = "TM_Skill_")]
public class SkillTM : ScriptableObject {

    public int typeID;
    public SkillType skillType;
    public string anim_Name;
    public bool isUseSword;

}