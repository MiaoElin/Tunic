using UnityEngine;

public class RoleFSMComponent {
    public RoleStatus status;
    public bool isEnterNormal;

    // Casting
    public bool isEnterCasting;
    public SkillCastStage skillCastStage;
    public float precastTimer;
    public float castingMaintainTimer;
    public float castingIntervalTimer;
    public float endCastTimer;

    // Suffering
    public bool isEnterSuffering;

    // Ladder
    public bool isEnterLadder;


    public void EnterNormal() {
        status = RoleStatus.Normal;
        isEnterNormal = true;
    }

    public void EnterCasting() {
        status = RoleStatus.Casting;
        isEnterCasting = true;
    }

    public void EnterSuffering() {
        status = RoleStatus.Suffering;
        isEnterSuffering = true;
    }

    public void EnterLadder() {
        status = RoleStatus.Ladder;
        isEnterLadder = true;
    }
}