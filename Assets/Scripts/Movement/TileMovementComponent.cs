using UnityEngine;

public class TileMovementComponent : MovementComponentBase {
    public override void GetMoves(MoveSet moveSet, Vector3Int unitPosition, TileComponent tileComponent) {
        var movesSize = MoveMap.size;
        var movesOrigin = MoveMap.origin;

        for (var y = 0; y < movesSize.y; y++)
        for (var x = 0; x < movesSize.x; x++) {
            var tilePos = new Vector3Int(x, y) + movesOrigin;

            if (tilePos is { x: 0, y: 0 }) {
                // Don't include current unit position
                continue;
            }

            var moveTilePos = tilePos + unitPosition;
            if (!tileComponent.IsUnobstructedTile(moveTilePos)) {
                // Skip obstructed tiles
                continue;
            }

            var tile = MoveMap.GetTile(tilePos);
            if (tile) {
                // TODO: Check tile to determine if move is jump
                moveSet.NormalMoves.Add(moveTilePos);
            }
        }
    }
}