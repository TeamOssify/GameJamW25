using UnityEngine;

public static class Bfs {
    public static void FilterMoves(SwapBackArray<(Vector3Int pos, MoveType type)> moves, TileComponent tile) {
        // TODO: Flood fill with BFS and remove disconnected normal moves
    }

    private record BfsNode {
        public BfsNode Parent;
        public Vector3Int Position;
    }
}