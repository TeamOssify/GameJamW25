using System.Collections.Generic;
using UnityEngine;

public class LongTileMovementComponent : MovementComponentBase {
    private readonly List<Vector3Int> _moveDirections = new();

    protected new void Awake() {
        base.Awake();

        InitMoveDirections();
    }

    private void InitMoveDirections() {
        var movesSize = MoveMap.size;
        var movesOrigin = MoveMap.origin;

        for (var y = 0; y < movesSize.y; y++)
        for (var x = 0; x < movesSize.x; x++) {
            var tilePos = new Vector3Int(x, y) + movesOrigin;

            if (tilePos is { x: 0, y: 0 }) {
                // Don't include current unit position
                continue;
            }

            if (MoveMap.HasTile(tilePos)) {
                _moveDirections.Add(tilePos);
            }
        }
    }

    public override void GetMoves(SwapBackArray<(Vector3Int pos, MoveType type)> existingMoves, Vector3Int unitPosition, TileComponent tileComponent) {
        foreach (var direction in _moveDirections) {
            // TODO: We Might not want to normalize to allow for long jumpy movement maps
            var dirNormalized = new Vector3Int(
                Mathf.Clamp(direction.x, -1, 1),
                Mathf.Clamp(direction.y, -1, 1)
            );

            if (dirNormalized == Vector3Int.zero) {
                Debug.LogWarning("Got zero vector! Bypassing infinite loop.");
                continue;
            }

            var nextPos = unitPosition + dirNormalized;
            while (tileComponent.IsUnobstructedTile(nextPos)) {
                existingMoves.Add((nextPos, MoveType.Normal));
                nextPos += dirNormalized;
            }
        }
    }
}