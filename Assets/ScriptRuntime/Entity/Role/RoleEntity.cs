using System;
using UnityEngine;

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
    public GameObject body;
    [SerializeField] Rigidbody rb;
    public Animator anim;

    // Com
    public WeaponComponent weaponCom;
    public StuffComponent stuffCom;

    public float searchRange;

    // Input
    public bool isMeleeKeyDown;
    public bool isShieldKeyPress;
    public bool isRangedKeyDown;
    public bool isJumpKeyDown;
    public bool isInteractKeyDown;

    // fsm
    public RoleFSMComponent fsm;

    public void Ctor(GameObject mod) {
        fsm = new RoleFSMComponent();
        weaponCom = new WeaponComponent();
        stuffCom = new StuffComponent();

        rotationSpeed = 10;
        // Body 生成
        body = GameObject.Instantiate(mod, transform);
        this.anim = body.GetComponentInChildren<Animator>();
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

    public Transform GetWeaponTrans(WeaponType weaponType) {
        if (weaponType == WeaponType.Melee || weaponType == WeaponType.Shooter) {
            return GetTransform("weaponShield_r", body.transform);
        }
        if (weaponType == WeaponType.Shield) {
            return GetTransform("weaponShield_l", body.transform);
        }
        return null;
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
        Anim_SetSpeed();
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
        anim.ResetTrigger("T_Defend");
        anim.CrossFade("Idle", 0);
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
}