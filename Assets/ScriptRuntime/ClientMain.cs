using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ClienMain : MonoBehaviour {
    [SerializeField] Transform realCamera;
    [SerializeField] CinemachineFreeLook mainCamera;
    [SerializeField] Canvas hudCanvas;
    [SerializeField] Canvas screenCanvas;
    bool isTearDown;
    GameContext ctx = new GameContext();
    void Start() {

        // Load
        Load();

        // Inject
        ctx.Inject(mainCamera, hudCanvas, screenCanvas);

        // PoolService
        ctx.poolService.Init(() => Factory.Role_Create(ctx), () => Factory.Weapon_Create(ctx), () => Factory.Loot_Create(ctx));

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
