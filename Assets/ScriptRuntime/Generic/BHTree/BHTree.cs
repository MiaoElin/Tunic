using UnityEngine;
public class BHTree {
    public BHTreeNode root;
    public bool isPause;

    public void InitRoot(BHTreeNode root) {
        this.root = root;
    }

    public void Pause() {
        isPause = true;
    }

    public void Resume() {
        isPause = false;
    }

    public void Execute(float dt) {
        if (isPause) {
            return;
        }
        Debug.Assert(root != null);
        BHTreeNodeStatus status = root.Execute(dt);
        if (status == BHTreeNodeStatus.Done) {
            root.Reset();
        }
    }
}