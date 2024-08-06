using UnityEngine;

public class RoleAIComponent {
    public BHTree tree;

    public EntityType entityType;
    public int targetID;

    public RoleAIComponent() {
        tree = new BHTree();
    }
}