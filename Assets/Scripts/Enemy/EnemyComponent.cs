using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyComponent : MonoBehaviour {
    [SerializeField]
    private string enemyName;

    [SerializeField]
    private MovementComponentBase enemyBaseMoves;

    [SerializeField]
    private MovementComponentBase enemyFirstMoves;

    [SerializeField]
    private MovementComponentBase enemyTier1Moves;

    public Vector3Int GridPos { get; private set; }
    public Vector3Int NextMove { get; private set; }

    public Vector3Int ComputeNextMove(TileComponent tileComponent, IEnumerable<Vector3Int> objectives, IEnumerable<Vector3Int> playerUnits, IReadOnlyCollection<Vector3Int> takenPositions) {
        var pointsOfInterest = new HashSet<Vector3Int>(objectives);
        pointsOfInterest.AddRange(playerUnits);

        var path = Bfs.NearestPointOfInterest(GridPos, pointsOfInterest, tileComponent);
        if (path is null) {
            Debug.Log("No path available.");
            return GridPos;
        }

        var moveSet = new MoveSet();
        enemyBaseMoves.GetMoves(moveSet, GridPos, tileComponent);

        var allMoves = moveSet.NormalMoves.Concat(moveSet.JumpMoves).ToArray();

        var closestPoint = GridPos;
        foreach (var pos2 in allMoves) {
            if (takenPositions.Contains(pos2)) {
                continue;
            }

            if (pos2.DistanceSquared(path[0]) < closestPoint.DistanceSquared(path[0])) {
                closestPoint = pos2;
            }
        }

        NextMove = closestPoint;
        return NextMove;
    }

    public void Move(Vector3 pos, Vector3Int gridPos) {
        transform.position = pos;
        GridPos = gridPos;
    }
}