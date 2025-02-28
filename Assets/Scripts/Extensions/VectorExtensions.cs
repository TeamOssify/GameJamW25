using UnityEngine;

public static class VectorExtensions {
    public static int DistanceSquared(this Vector3Int a, Vector3Int b) {
        return (a - b).sqrMagnitude;
    }
}