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
        body = GameObject.Instantiate(mod, transform);
        this.anim = body.GetComponentInChildren<Animator>();
    }

    #region Pos
    public void SetPos(Vector2 pos) {
        transform.position = pos;
    }

    public Vector2 Pos() {
        return transform.position;
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