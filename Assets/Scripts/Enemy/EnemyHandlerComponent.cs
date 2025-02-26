using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandlerComponent : MonoBehaviour {
    [SerializeField]
    private TileComponent tileComponent;

    private Vector3Int[] _pointsOfInterest;

    private Dictionary<Vector3Int, EnemyComponent> _enemyGridPositions = new();
    private Dictionary<Vector3Int, EnemyComponent> _futureEnemyGridPositions = new();

    public void UpdatePointsOfInterest() {

    }

    public void ComputeEnemyMoves() {
        var takenPoints = new HashSet<Vector3Int>();
        _futureEnemyGridPositions.Clear();

        foreach (var enemy in _enemyGridPositions.Values) {
            var movePoint = enemy.ComputeNextMove(tileComponent, _pointsOfInterest, Array.Empty<Vector3Int>(), takenPoints);
            takenPoints.Add(movePoint);
            _futureEnemyGridPositions.Add(movePoint, enemy);
        }
    }

    public void MoveEnemies() {
        foreach (var (gridPos, enemy) in _futureEnemyGridPositions) {
            if (!tileComponent.TryGetWorldPositionForTileCenter(gridPos, out var worldPos)) {
                Debug.LogError($"Failed to get new world pos for enemy at {gridPos}!");
                continue;
            }

            enemy.Move(worldPos, gridPos);
        }

        (_enemyGridPositions, _futureEnemyGridPositions) = (_futureEnemyGridPositions, _enemyGridPositions);
    }
}