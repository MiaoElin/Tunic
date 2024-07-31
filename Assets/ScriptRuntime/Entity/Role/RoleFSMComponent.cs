using System;
using UnityEngine;

public class RoleFSMComponent {
    public RoleStatus status;
    public bool isEnterNormal;

    // Casting
    public bool isEnterCasting;
    public SkillCastStage skillCastStage;
    public bool isResetCastSkill;
    public int comboCount;

    public float precastTimer;
    public float castingMaintainTimer;
    public float castingIntervalTimer;
    public float endCastTimer;

    // === Suffering ===
    public bool isEnterSuffering;
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

    // === Ladder === 
    public bool isEnterLadder;

    // === Dead ===
    public bool isEnterDead;


    public void EnterNormal() {
        status = RoleStatus.Normal;
        isEnterNormal = true;
    }

    public void EnterCasting() {
        status = RoleStatus.Casting;
        isEnterCasting = true;
        isResetCastSkill = true;
    }

    public void ResetCastSkill(SkillSubEntity skill) {
        skillCastStage = SkillCastStage.PreCast;
        precastTimer = skill.precastCDMax;
        castingMaintainTimer = skill.castingMaintainSec;
        castingIntervalTimer = skill.castingIntervalSec;
        endCastTimer = skill.endCastSec;
    }

    public void EnterSuffering(float stiffSec, float hitBackForce, float hitBackSec, float hitUpForce, float hitUpSec) {
        status = RoleStatus.Suffering;
        isEnterSuffering = true;
        this.stiffSec = stiffSec;
        this.hitBackForce = hitBackForce;
        this.hitBackSec = hitBackSec;
        this.hitUpSec = hitUpSec;
        this.hitUpForce = hitUpForce;
    }

    public void EnterLadder() {
        status = RoleStatus.Ladder;
        isEnterLadder = true;
    }

    internal void EnterDead() {
        status = RoleStatus.Dead;
        isEnterDead = true;
    }
}