using UnityEngine;
public class BHTree {
    public RoleAINodeModel root;
    public bool isPause;

    public void InitRoot(RoleAINodeModel root) {
        this.root = root;
    }

    public void Pause() {
        isPause = true;
    }

    public void Resume() {
        isPause = false;
    }

    public void Execute(float dt) {
        if (root == null) {
            return;
        }
        if (isPause) {
            return;
        }
        RoleAINodeStatus status = root.Execute(dt);
        if (status == RoleAINodeStatus.Done) {
            root.Reset();
        }
    }
}