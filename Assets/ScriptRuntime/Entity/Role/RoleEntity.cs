using System;
using UnityEngine;

public class RoleEntity : MonoBehaviour {

    public int typeID;
    public int id;
    public float moveSpeed;
    public float rotationSpeed;
    public Ally ally;
    public GameObject body;
    [SerializeField] Rigidbody rb;
    public Animator anim;

    // Skill
    public SkillComponent skillCom;

    // Input
    public bool isSwordKeyDown;
    public bool isShieldKeyDown;
    public bool isRangedKeyDown;
    public bool isJumpKeyDown;
    public bool isInteractKeyDown;

    // fsm
    public RoleFSMComponent fsm;

    public void Ctor(GameObject mod) {
        skillCom = new SkillComponent();
        fsm = new RoleFSMComponent();

        rotationSpeed = 10;
        // Body 生成
        body = GameObject.Instantiate(mod, transform);
        this.anim = body.GetComponentInChildren<Animator>();
    }

    public void Reuse() {

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
    #endregion

    #region ForWard
    public void SetForward(Vector3 dir, float dt) {
        body.transform.forward = Vector3.Lerp(body.transform.forward, dir, dt * rotationSpeed);
    }
    #endregion

    #region Anim
    public void Anim_SetSpeed() {
        anim.SetFloat("F_MoveSpeed", rb.velocity.magnitude);
    }

    public void Anim_Attack() {

    }
    #endregion

    #region Input
    public void UpdateInputKey(bool isSwordKeyDown, bool isShieldKeyDown, bool isRangedKeyDown, bool isJumpKeyDown, bool isInteractKeyDown) {
        this.isSwordKeyDown = isSwordKeyDown;
        this.isShieldKeyDown = isShieldKeyDown;
        this.isRangedKeyDown = isRangedKeyDown;
        this.isJumpKeyDown = isJumpKeyDown;
        this.isInteractKeyDown = isInteractKeyDown;
    }
    #endregion
}