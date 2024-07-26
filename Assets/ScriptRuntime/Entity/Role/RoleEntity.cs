using System;
using UnityEngine;
using System.Collections.Generic;

public class RoleEntity : MonoBehaviour {

    public int typeID;
    public int id;
    public bool isOwner;
    public float moveSpeed;
    public float jumpForce;
    public int jumpTimes;
    public int jumpTimesMax;
    public float gravity;
    public float rotationSpeed;
    public Ally ally;
    public AiType aiType;
    public GameObject body;
    [SerializeField] Rigidbody rb;
    public Animator anim;
    public RoleAnimState animState;

    // Com
    public WeaponComponent weaponCom;
    public StuffComponent stuffCom;
    public RoleAIComponent aiCom;
    public bool hasTarget;
    public float searchRange;
    public float attackRange;

    // Input
    public bool isMeleeKeyDown;
    public bool isShieldKeyPress;
    public bool isRangedKeyDown;
    public bool isJumpKeyDown;
    public bool isInteractKeyDown;

    // fsm
    public RoleFSMComponent fsm;

    public List<Vector3> path;
    public int pathIndex;

    public void Ctor(GameObject mod) {
        path = new List<Vector3>();

        fsm = new RoleFSMComponent();
        weaponCom = new WeaponComponent();
        stuffCom = new StuffComponent();
        aiCom = new RoleAIComponent();

        rotationSpeed = 20;
        // Body 生成
        body = GameObject.Instantiate(mod, transform);
        this.anim = body.GetComponentInChildren<Animator>();
        animState = RoleAnimState.NoWeapon;
    }

    public void Reuse() {
        weaponCom.Foreach(weapon => {
            weaponCom.Remove(weapon);
            Destroy(weapon.gameObject);
        });
    }

    #region Pos
    public void SetPos(Vector3 pos) {
        transform.position = pos;
    }

    public Vector3 Pos() {
        return transform.position;
    }

    public void SetRotation(Vector3 rotation) {
        transform.eulerAngles = rotation;
    }

    public void SetLocalScale(Vector3 localScale) {
        transform.localScale = localScale;
    }

    public Transform GetWeaponTrans(WeaponType weaponType, string tranName) {
        return GetTransform(tranName, body.transform);
    }

    public Vector3 GetBody_Center() {
        return body.transform.Find("Body_Center").position;
    }

    public Transform GetTransform(String name, Transform trans) {

        var target = trans.Find(name);
        if (target != null) {
            return target;
        }
        for (int i = 0; i < trans.childCount; i++) {
            var child = trans.GetChild(i);
            target = GetTransform(name, child);
            if (target != null) {
                return target;
            }
        }
        return null;
    }
    #endregion

    #region  Move
    public void Move(Vector3 moveAxis, float dt) {
        var velocity = rb.velocity;
        velocity = moveAxis.normalized * moveSpeed;
        velocity.y = rb.velocity.y;
        rb.velocity = velocity;
        if (moveAxis != Vector3.zero) {
            SetForward(moveAxis, dt);
        }
    }

    public void Move_Stop() {
        var velocity = rb.velocity;
        velocity = Vector3.zero;
        velocity.y = rb.velocity.y;
        rb.velocity = velocity;
        Anim_SetSpeedZero();
    }
    public void AI_Move_Stop() {
        var velocity = rb.velocity;
        velocity = Vector3.zero;
        rb.velocity = velocity;
        Anim_SetSpeedZero();
    }

    internal void MoveTo_Target(Vector3 target, float dt) {
        var dir = target - Pos();
        if (Vector3.SqrMagnitude(dir) < Math.Pow(moveSpeed * dt, 2)) {
            rb.velocity = Vector3.zero;
            return;
        }
        var velocity = rb.velocity;
        velocity = dir.normalized * moveSpeed;
        rb.velocity = velocity;
        SetForward(dir, dt);
    }

    public void MoveBy_Path(float dt) {
        var velocity = rb.velocity;
        if (path == null) {
            return;
        }
        // 是否到达终点
        if (pathIndex >= path.Count) {
            velocity = Vector3.zero;
            rb.velocity = velocity;
            pathIndex = 0;
            return;
        }
        // 移动
        var dir = path[pathIndex] - Pos();
        velocity = moveSpeed * dir.normalized;
        rb.velocity = velocity;
        // 到达当前Index 的目标位置
        if (Vector3.SqrMagnitude(dir) < Mathf.Pow(moveSpeed * dt, 2)) {
            pathIndex++;
            return;
        }
        dir.y = 0;
        SetForward(dir, dt);
    }

    #endregion

    #region ForWard
    public void SetForward(Vector3 dir, float dt) {
        body.transform.forward = Vector3.Lerp(body.transform.forward, dir, dt * rotationSpeed);
    }

    public Vector3 GetForward() {
        return body.transform.forward;
    }
    #endregion

    #region Jump
    public void Jump() {
        if (isJumpKeyDown && jumpTimes > 0) {
            jumpTimes -= 1;
            var velocity = rb.velocity;
            velocity.y = jumpForce;
            rb.velocity = velocity;
            Anim_JumpStart();
        }
    }

    public void ResetJumpTimes() {
        if (jumpTimes != jumpTimesMax) {
            Anim_JumpEnd();
            jumpTimes = jumpTimesMax;
        }
    }

    public void Falling(float dt) {
        var velocity = rb.velocity;
        velocity.y -= gravity * dt;
        rb.velocity = velocity;
    }
    #endregion

    #region Velocity
    public float GetVelocityY() {
        return rb.velocity.y;
    }
    #endregion

    #region Anim
    public void Anim_SetSpeed() {
        anim.SetFloat("F_MoveSpeed", rb.velocity.magnitude);
    }

    public void Anim_SetSpeedZero() {
        anim.SetFloat("F_MoveSpeed", 0);
    }

    public void Anim_Attack(string anim_Name) {
        if (anim_Name == "") {
            return;
        }
        anim.CrossFade(anim_Name, 0);
    }

    internal void Anim_Defend(bool b) {
        anim.SetBool("B_Defend", b);
    }

    internal void Anim_Idle() {
        if (isOwner) {
            if (animState == RoleAnimState.NoWeapon) {
                anim.SetFloat("F_AnimState", 0);
            } else if (animState == RoleAnimState.SwordAndShield) {
                anim.SetFloat("F_AnimState", 1);
            }
        }
    }

    public void Anim_JumpStart() {
        anim.CrossFade("JumpStart_SwordShield", 0);
    }

    public void Anim_JumpEnd() {
        anim.CrossFade("JumpEnd_SwordShield", 0);
    }

    #endregion

    #region Input
    public void UpdateInputKey(bool isMeleeKeyDown, bool isShieldKeyPress, bool isRangedKeyDown, bool isJumpKeyDown, bool isInteractKeyDown) {
        this.isMeleeKeyDown = isMeleeKeyDown;
        this.isShieldKeyPress = isShieldKeyPress;
        this.isRangedKeyDown = isRangedKeyDown;
        this.isJumpKeyDown = isJumpKeyDown;
        this.isInteractKeyDown = isInteractKeyDown;
    }
    #endregion

    #region WeaponCom
    public void AddWeapon(WeaponEntity weapon) {
        weaponCom.Add(weapon);
        animState = RoleAnimState.SwordAndShield;
        Anim_Idle();
    }

    public void SetCatingWeapon(WeaponEntity weapon) {
        weaponCom.SetCatingWeapon(weapon);
    }

    #endregion
}