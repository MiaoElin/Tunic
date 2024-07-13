using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class RoleEM : MonoBehaviour {
    public RoleTM tm;
    GameObject mod;

    void Awake() {
        if (mod == null) {
            mod = GameObject.Instantiate(tm.mod, transform);
        }
    }
}