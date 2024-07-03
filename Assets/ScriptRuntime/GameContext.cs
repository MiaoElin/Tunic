using UnityEngine;

public class GameContext {

    public float resetTime;

    // === Service ===
    public IDService iDService;

    // === Repo ===
    public RoleRepo roleRepo;

    // === Entity ===
    public GameEntity game;
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
        input = new InputEnitty();
        // Core
        asset = new Asset_Core();
    }

    public RoleEntity GetOwner() {
        roleRepo.TryGet(game.ownerID, out var owner);
        return owner;
    }

}