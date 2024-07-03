using UnityEngine;

public class RoleEntity : MonoBehaviour {

    public int typeID;
    public int id;
    public float moveSpeed;
    public Ally ally;
    public GameObject body;
    [SerializeField] Rigidbody rb;
    public Animator anim;

    public void Ctor(GameObject mod) {
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

    #region Anim
    public void Anim_SetSpeed() {
        anim.SetFloat("F_MoveSpeed", rb.velocity.magnitude);
    }

    public void Anim_Attack() {

    }

    #endregion

}