using UnityEngine;

public class MoveSet {
    public readonly SwapBackArray<Vector3Int> NormalMoves = new();
    public readonly SwapBackArray<Vector3Int> JumpMoves = new();
}