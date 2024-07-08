using UnityEngine;

public class InputEnitty {

    public Vector3 moveAxis;
    public bool isMeleeKeyDown;
    public bool isShieldKeyPress;
    public bool isRangedKeyDown;
    public bool isJumpKeyDown;
    // public bool isRollingKeyDown;
    public bool isInteractKeyDown;
    public bool isBagKeyDown;

    public InputEnitty() {

    }

    public void Process(Vector3 cameraForward, Vector3 cameraRight) {
        moveAxis = Vector3.zero;
        if (Input.GetKey(KeyCode.A)) {
            moveAxis.x = -1;
        } else if (Input.GetKey(KeyCode.D)) {
            moveAxis.x = 1;
        }

        if (Input.GetKey(KeyCode.W)) {
            moveAxis.z = 1;
        } else if (Input.GetKey(KeyCode.S)) {
            moveAxis.z = -1;
        }
        cameraForward.y = 0;
        cameraRight.y = 0;
        moveAxis = cameraForward * moveAxis.z + cameraRight * moveAxis.x;
        Vector3.Normalize(moveAxis);

        // 攻击的优先级在盾之前
        // 用剑
        if (Input.GetMouseButtonDown(0)) {
            isMeleeKeyDown = true;
        } else {
            isMeleeKeyDown = false;
        }
        // 1.与剑只能二选一发射、不能同时发射
        // 2.远距离丢东西/有锁定目标朝目标方向丢，没有的朝角色前方
        if (Input.GetKeyDown(KeyCode.F)) {
            isRangedKeyDown = true;
        } else {
            isRangedKeyDown = false;
        }

        // 盾
        if (Input.GetMouseButton(1)) {
            // 可以长按 
            isShieldKeyPress = true;
        } else {
            isShieldKeyPress = false;
        }

        // Interact
        if (Input.GetKeyDown(KeyCode.E)) {
            isInteractKeyDown = true;
        } else {
            isInteractKeyDown = false;
        }

        // Jump
        if (Input.GetKeyDown(KeyCode.Space)) {
            isJumpKeyDown = true;
        } else {
            isJumpKeyDown = false;
        }

        // Bag
        isBagKeyDown = Input.GetKeyDown(KeyCode.Tab);
    }
}