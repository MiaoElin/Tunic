using System;
using UnityEngine;

public class BaseSlotEntity : MonoBehaviour {

    public int typeID;
    public int id;
    public BaseSlotType baseSlotType;
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
