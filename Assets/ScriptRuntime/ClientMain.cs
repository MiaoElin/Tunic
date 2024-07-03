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
        var role = RoleDomain.Spawn(ctx, 100, new Vector3(60, 0, 10), Vector3.zero, Vector3.one, Ally.Player);
        ctx.game.ownerID = role.id;
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

        ctx.input.Process();

        var owner = ctx.GetOwner();
        RoleDomain.Owner_Move(ctx, owner);
    }
}
