using System;
using UnityEngine;
using UnityEngine.UI;

public class HUD_Hints : MonoBehaviour {
    [SerializeField] Image icon;
    public int id;

    public void Show() {
        gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }

    internal void SetPos(Vector3 worldPos) {
        transform.position = worldPos;
    }

    public void SetForward(Vector3 target) {
        Vector3 dir = transform.position - target;
        transform.forward = dir.normalized;
    }
}