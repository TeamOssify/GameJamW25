using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyComponent : MonoBehaviour {
    [SerializeField]
    private string enemyName;

    [SerializeField]
    private Tilemap enemyMoves;

    public Vector3Int GridPos { get; private set; }
    public Vector3Int NextMove { get; private set; }

    public void Setup(Vector3Int startingPos) {
        GridPos = startingPos;
    }

    public Vector3Int ComputeNextMove(TileComponent tilemap, IEnumerable<Vector3Int> pointsOfInterest, IEnumerable<Vector3Int> playerUnits, IReadOnlyCollection<Vector3Int> takenPositions) {
        // TODO: Compute path to POI
        // TODO: Compare path to nearby units & target higher priority
        NextMove = GridPos;

        return NextMove;
    }

    public void Move(Vector3 pos, Vector3Int gridPos) {
        transform.position = pos;
        GridPos = gridPos;
    }
}