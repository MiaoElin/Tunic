using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ClienMain : MonoBehaviour {
    [SerializeField] Transform realCamera;
    [SerializeField] CinemachineFreeLook mainCamera;
    bool isTearDown;
    GameContext ctx = new GameContext();
    void Start() {

        // Inject
        ctx.Inject(mainCamera);

        // Load
        Load();
        GameBusiness_Normal.EnterStage(ctx, 0);
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
        var dt = Time.deltaTime;
        ctx.input.Process(realCamera.forward, realCamera.right);
        var status = ctx.game.fsm.status;
        if (status == GameStatus.Login) {

        } else if (status == GameStatus.Normal) {
            GameBusiness_Normal.Tick(ctx, dt);
        }
    }
}
