using System;
using UnityEngine;

public class LootEntity : MonoBehaviour {

    public int id;
    public int typeID;
    public GameObject mod;
    public int[] stufftypeIDs;
    public int[] stuffCounts;


    public void Ctor(GameObject mod) {
        this.mod = GameObject.Instantiate(mod, transform);
    }

    public void Reuse() {
        Destroy(mod.gameObject);
    }

    internal void SetPos(Vector3 pos) {
        transform.position = pos;
    }

    public Vector3 Pos() {
        return transform.position;
    }

    internal void SetRotaion(Vector3 rotation) {
        transform.eulerAngles = rotation;
    }

    internal void SetLocalScale(Vector3 localScale) {
        transform.localScale = localScale;
    }
}