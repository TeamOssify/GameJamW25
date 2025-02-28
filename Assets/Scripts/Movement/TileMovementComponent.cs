using UnityEngine;

public class TileMovementComponent : MovementComponentBase {
    [SerializeField]
    private bool isJumpMap;

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

            if (MoveMap.HasTile(tilePos)) {
                if (isJumpMap) {
                    moveSet.JumpMoves.Add(moveTilePos);
                }
                else {
                    moveSet.NormalMoves.Add(moveTilePos);
                }
            }
        }
    }
}