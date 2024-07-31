using UnityEngine;
using TriInspector;

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

    [Title("HitBox")]
    public HitBoxType hitBoxType;
    public Vector3 boxSize;
    // public Quaternion boxRot;

    // 静止帧
    public float hitLockSec;
    // 僵直时间
    public float stiffSec;
    // 击退时间
    public float hitBackSec;
    // 击退力度
    public float hitBackForce;
    // 击飞
    public float hitUpSec;
    public float hitUpForce;

}