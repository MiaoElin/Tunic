using UnityEngine;

[CreateAssetMenu(menuName = "TM/TM_Skill", fileName = "TM_Skill_")]
public class SkillTM : ScriptableObject {

    public int typeID;
    public InputKeyEnum inputKeyEnum;
    public string anim_Name;
    public float cdMax;

    public float precastCDMax;

    public float castingMaintainSec;
    public float castingIntervalSec;

    public float endCastSec;

    public bool canCombo;
    public SkillTM comboSkillTM;

}