using UnityEngine;

public class InputEnitty {

    public Vector3 moveAxis;

    public InputEnitty() {

    }

    public void Process() {
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
    }
}