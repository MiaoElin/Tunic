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
    public GameEntity game;
    public CameraEntity camera;
    public InputEnitty input;

    // === Core === 
    public Asset_Core asset;

    public GameContext() {
        // Service
        iDService = new IDService();
        poolService = new PoolService();
        // Repo
        roleRepo = new RoleRepo();
        lootRepo = new LootRepo();
        // Entity
        game = new GameEntity();
        camera = new CameraEntity();
        input = new InputEnitty();
        // Core
        asset = new Asset_Core();
    }

    public void Inject(CinemachineFreeLook mainCamera) {
        camera.Inject(mainCamera);
    }

    public RoleEntity GetOwner() {
        roleRepo.TryGet(game.ownerID, out var owner);
        return owner;
    }

}