using UnityEngine;

public class WeaponEntity : MonoBehaviour {
    public int id;
    public int typeID;
    public WeaponType weaponType;
    public GameObject mod;
    public int bulletTypeID;//炸弹和子弹都是bullet

    public void Ctor(GameObject mod) {
        this.mod = GameObject.Instantiate(mod, transform);
    }
}