using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClienMain : MonoBehaviour {
    // [SerializeField] 
    bool isTearDown;
    GameContext ctx = new GameContext();
    void Start() {
        // Load
        Load();
        RoleDomain.Spawn(ctx, 100, new Vector3(60, 0, 60), Vector3.zero, Vector3.one, Ally.Player);
    }

    public void Load() {
        ctx.asset.LoadAll();
    }

    void OnApplicationQuit() {
        Unload();
    }

    void OnDestory() {
        Unload();
    }

    public void Unload() {
        if (isTearDown) {
            return;
        }
        isTearDown = true;
        ctx.asset.Unload();
    }

    void Update() {

    }
}
