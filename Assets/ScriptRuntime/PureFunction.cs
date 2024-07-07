using UnityEngine;

public static class PureFunction {

    public static bool IsPostInRange(Vector3 target, Vector3 center, float range) {
        float distance = Vector3.SqrMagnitude(target - center);
        if (distance <= range * range) {
            return true;
        } else {
            return false;
        }
    }
}