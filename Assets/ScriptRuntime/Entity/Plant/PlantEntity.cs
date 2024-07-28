using System;
using UnityEngine;

public class PlantEntity : MonoBehaviour {

    public int typeID;
    public int id;
    public PlantType plantType;
    public GameObject mod;

    public void Ctor(GameObject mod) {
        this.mod = GameObject.Instantiate(mod, transform);
    }

    public void SetPos(Vector3 pos) {
        transform.position = pos;
    }

    internal void Reuse() {
        Destroy(mod.gameObject); 
    }

}
