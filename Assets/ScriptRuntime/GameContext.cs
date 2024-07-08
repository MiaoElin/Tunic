using UnityEngine;
using Cinemachine;

public class GameContext {

    public float resetTime;

    // === Service ===
    public IDService iDService;
    public PoolService poolService;

    // === Repo ===
    public RoleRepo roleRepo;
    public LootRepo lootRepo;

    // === Entity ===
    public PlayerEntity player;
    public GameEntity game;
    public CameraEntity camera;
    public InputEnitty input;

    // === Core === 
    public Asset_Core asset;
    public UIApp uIApp;

    public GameContext() {
        // Service
        iDService = new IDService();
        poolService = new PoolService();
        // Repo
        roleRepo = new RoleRepo();
        lootRepo = new LootRepo();
        // Entity
        player = new PlayerEntity();
        game = new GameEntity();
        camera = new CameraEntity();
        input = new InputEnitty();
        // Core
        asset = new Asset_Core();
        uIApp = new UIApp();
    }

    public void Inject(CinemachineFreeLook mainCamera, Canvas hudCanvas, Canvas screenCanvas) {
        camera.Inject(mainCamera);
        uIApp.Inject(hudCanvas, screenCanvas, asset);
    }

    public RoleEntity GetOwner() {
        roleRepo.TryGet(game.ownerID, out var owner);
        return owner;
    }

}