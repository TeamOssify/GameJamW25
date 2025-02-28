using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CapturePointHandler : MonoBehaviour {
    [SerializeField]
    private CapturePoint capturePointPrefab;

    [SerializeField]
    private TileComponent tileComponent;

    [SerializeField]
    private EnemyHandler enemyHandler;

    [SerializeField]
    private Vector3Int[] captureSpawns;

    public UnityEvent allPointsCaptured;

    public IEnumerable<Vector3Int> CapturePointPositions => _capturePointPositions;

    private readonly HashSet<Vector3Int> _capturePointPositions = new();

    private void Start() {
        foreach (var gridPos in captureSpawns) {
            AddCapturePoint(gridPos);
        }
    }

    public void AddCapturePoint(Vector3Int gridPos) {
        if (!tileComponent.TryGetWorldPositionForTileCenter(gridPos, out var worldPos)) {
            Debug.LogWarning($"Failed to get world pot for capture point at {worldPos}!");
        }

        if (!tileComponent.IsUnobstructedTile(gridPos)) {
            Debug.LogWarning($"Tried to place capture point on obstructed tile at {gridPos}!");
            return;
        }

        var newPoint = Instantiate(capturePointPrefab, transform.position, Quaternion.identity);
        newPoint.Move(worldPos, gridPos);

        _capturePointPositions.Add(gridPos);

        enemyHandler.enemiesMoved += newPoint.OnEnemyMove;
        newPoint.captured.AddListener(() => OnPointCaptured(newPoint));
    }

    public void OnPointCaptured(CapturePoint capturePoint) {
        _capturePointPositions.Remove(capturePoint.GridPosition);
        enemyHandler.enemiesMoved -= capturePoint.OnEnemyMove;

        if (_capturePointPositions.Count == 0) {
            allPointsCaptured.Invoke();
        }
    }

    public bool IsCapturePoint(Vector3Int gridPos) {
        return _capturePointPositions.Contains(gridPos);
    }
}