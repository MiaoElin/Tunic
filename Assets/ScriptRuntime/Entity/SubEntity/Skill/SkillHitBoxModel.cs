using UnityEngine;

public class SkillHitBoxModel {
    public HitBoxType hitBoxType;
    public Vector3 size;
    // public Quaternion rot;

    public float baseDamage;

    // 静止帧
    public float hitLockSec;
    // 僵直时间
    public float stiffSec;
    // 击退时间
    public float hitBackSec;
    // 击退力度
    public float hitBackForce;

    public SkillHitBoxModel() {

    }
}