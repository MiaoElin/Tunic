using System;
using UnityEngine;

public class LootEntity : MonoBehaviour {

    public int typeID;
    public int id;
    public GameObject mod;

    public void Ctor(GameObject mod) {
        this.mod = GameObject.Instantiate(mod, transform);
    }

    public void Reuse() {

    }

    internal void SetPo(Vector3 pos) {
        transform.position = pos;
    }

    internal void SetRotaion(Vector3 rotation) {
        transform.eulerAngles = rotation;
    }

    internal void SetLocalScale(Vector3 localScale) {
        transform.localScale = localScale;
    }
}