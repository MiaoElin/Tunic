using UnityEngine;
using Cinemachine;

public class GameContext {

    public float resetTime;

    // === Service ===
    public IDService iDService;

    // === Repo ===
    public RoleRepo roleRepo;

    // === Entity ===
    public GameEntity game;
    public CameraEntity camera;
    public InputEnitty input;

    // === Core === 
    public Asset_Core asset;

    public GameContext() {
        // Service
        iDService = new IDService();
        // Repo
        roleRepo = new RoleRepo();
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