using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class BaseSlotEM : MonoBehaviour {
    public BaseSlotTM tm;
    GameObject mod;

    void Awake() {
        if (mod == null) {
            mod = GameObject.Instantiate(tm.mod, transform);
        }
    }

}