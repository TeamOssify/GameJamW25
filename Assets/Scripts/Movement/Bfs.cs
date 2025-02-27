using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public static class Bfs {
    /// <returns>The positions not visited by the flood fill</returns>
    public static HashSet<Vector3Int> FloodFill(Vector3Int startingPos, IEnumerable<Vector3Int> toVisit, TileComponent tileComponent) {
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

    /// <returns>The path to the nearest point of interest</returns>
    [return: MaybeNull]
    public static List<Vector3Int> NearestPointOfInterest(Vector3Int startingPos, HashSet<Vector3Int> pointsOfInterest, TileComponent tileComponent) {
        var visited = new HashSet<Vector3Int>();
        var queue = new Queue<BfsNode>();

        queue.Enqueue(new BfsNode {
            Parent = null,
            Pos = startingPos
        });

        while (queue.TryDequeue(out var current)) {
            for (var y = -1; y <= 1; y++)
            for (var x = -1; x <= 1; x++) {
                if (x == 0 && y == 0) {
                    continue;
                }

                var newPoint = new Vector3Int(current.Pos.x + x, current.Pos.y + y, 0);
                if (!tileComponent.IsValidTile(newPoint) || !visited.Add(newPoint)) {
                    continue;
                }

                var newNode = new BfsNode {
                    Parent = current,
                    Pos = newPoint
                };

                if (pointsOfInterest.Contains(newPoint)) {
                    return RetracePath(newNode);
                }

                queue.Enqueue(newNode);
            }
        }

        return null;
    }

    private static List<Vector3Int> RetracePath(BfsNode node) {
        var path = new List<Vector3Int>();

        var current = node;
        while (current != null) {
            path.Add(current.Pos);
            current = current.Parent;
        }

        path.Reverse();
        return path;
    }

    private record BfsNode {
        public BfsNode Parent;
        public Vector3Int Pos;
    }
}