using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class PlantEM : MonoBehaviour {
    public PlantTM tm;
    public GameObject mod;

    void Awake() {
        if (mod == null) {
            mod = GameObject.Instantiate(tm.mod, transform);
        }
    }

}