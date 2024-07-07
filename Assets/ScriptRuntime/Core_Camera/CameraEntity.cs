using UnityEngine;
using Cinemachine;
using System;

public class CameraEntity {

    public CinemachineFreeLook mainCamera;

    public void Inject(CinemachineFreeLook mainCamera) {
        this.mainCamera = mainCamera;
    }

    public Vector3 Pos() {
        return mainCamera.transform.position;
    }

    internal void SetFollow(Transform transform) {
        mainCamera.Follow = transform;
    }

    internal void SetLookAt(Transform transform) {
        mainCamera.LookAt = transform;
    }
}