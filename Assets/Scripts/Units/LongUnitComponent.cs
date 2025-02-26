using UnityEngine;
using System.Collections.Generic;

public class LongUnitComponent : UnitComponent {
    protected override SwapBackArray<Vector3Int> GetBaseMoves(TileComponent tileComponent) {
        var movesSize = unitBaseMoves.size;
        var movesOrigin = unitBaseMoves.origin;

        var moves = new SwapBackArray<Vector3Int>();

        var directions = new List<Vector3Int>();
        for (var y = 0; y < movesSize.y; y++)
        for (var x = 0; x < movesSize.x; x++) {
            var tilePos = new Vector3Int(x, y) + movesOrigin;

            if (tilePos is { x: 0, y: 0 }) {
                // Don't include current unit position
                continue;
            }

            if (unitBaseMoves.HasTile(tilePos)) {
                directions.Add(tilePos);
            }
        }

        foreach (var direction in directions) {
            var dirNormalized = new Vector3Int(
                Mathf.Clamp(direction.x, -1, 1),
                Mathf.Clamp(direction.y, -1, 1)
            );

            var nextPos = GridPos + dirNormalized;
            while (tileComponent.IsValidTile(nextPos)) {
                moves.Add(nextPos);
                nextPos += dirNormalized;
            }
        }

        return moves;
    }
}
