using System;
using System.Collections.Generic;
using UnityEngine;

public static class Bfs {
    public static void FilterMoves(Vector3Int unitPosition, SwapBackArray<Vector3Int> moves, TileComponent tileComponent) {
        var notVisited = BfsFloodFill(unitPosition, moves, tileComponent);

        moves.RemoveAll(x => notVisited.Contains(x));
    }

    private static HashSet<Vector3Int> BfsFloodFill(Vector3Int startingPos, IEnumerable<Vector3Int> toVisit, TileComponent tileComponent) {
        var notVisited = new HashSet<Vector3Int>(toVisit);
        var queue = new Queue<Vector3Int>();
        queue.Enqueue(startingPos);

        while (queue.TryDequeue(out var current)) {
            for (var y = -1; y <= 1; y++)
            for (var x = -1; x <= 1; x++) {
                if (x == 0 && y == 0) {
                    continue;
                }

                var newPoint = new Vector3Int(current.x + x, current.y + y, 0);
                if (!tileComponent.IsValidTile(newPoint) || !notVisited.Remove(newPoint)) {
                    continue;
                }

                queue.Enqueue(newPoint);
            }
        }

        return notVisited;
    }
}