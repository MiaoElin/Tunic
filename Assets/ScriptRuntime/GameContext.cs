using UnityEngine;

public class GameContext {

    // === Service ===
    public IDService iDService;

    // === Repo ===
    public RoleRepo roleRepo;

    // === Entity ===
    public GameEntity game;

    // === Core === 
    public Asset_Core asset;

    public GameContext() {
        // Service
        iDService = new IDService();
        // Repo
        roleRepo = new RoleRepo();
        // Entity
        game = new GameEntity();
        // Core
        asset = new Asset_Core();
    }

    public RoleEntity GetOwner() {
        roleRepo.TryGet(game.ownerID, out var owner);
        return owner;
    }

}