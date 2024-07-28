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

        Cursor.lockState = CursorLockMode.Locked;

        // Load
        Load();

        // Inject
        ctx.Inject(mainCamera, hudCanvas, screenCanvas);

        // PoolService
        ctx.poolService.Init(
            () => GameFactory.Role_Create(ctx),
            () => GameFactory.Weapon_Create(ctx),
            () => GameFactory.Loot_Create(ctx),
            () => GameFactory.Plant_Create(ctx));

        // Bind
        Bind();

        // Physics
        Physics.IgnoreLayerCollision(LayerMaskConst.ROLE, LayerMaskConst.ROLE); // 必须是碰撞盒在的那个Gameobject，父对象设了Layer.ROlE无效

        GameBusiness_Normal.EnterStage(ctx, 0);

    }

    private void Bind() {
        var eventCenter = ctx.eventCenter;
        eventCenter.OnClickBagGridHandle = (int typeID) => {
            // 使用stuff
            RoleDomain.Owner_UseStuff(ctx, typeID);
        };
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
        } else if (status == GameStatus.OpenBag) {
            GameBusiness_OpenBag.Tick(ctx, dt);
        }
    }
}
