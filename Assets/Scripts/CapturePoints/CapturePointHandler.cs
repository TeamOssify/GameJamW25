using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CapturePointHandler : MonoBehaviour {
    [SerializeField]
    private GameObject capturePointPrefab;

    public readonly HashSet<Vector3Int> CapturePointPositions = new();

    public TileComponent tileComponent;
    public EnemyHandler enemyHandler;
    public List<Vector3Int> captureSpawns;

    public UnityEvent allPointsCaptured;

    public void SpawnCapturePoints() {
        foreach (var i in captureSpawns) {
            AddCapturePoint(i);
        }
    }

    public void AddCapturePoint(Vector3Int i) {
        var newPoint = Instantiate(capturePointPrefab, transform);
        var pointComponent = newPoint.GetComponent<CapturePoint>();

        tileComponent.TryGetWorldPositionForTileCenter(i, out var pos);
        pointComponent.Move(pos, i);

        CapturePointPositions.Add(i);

        enemyHandler.enemiesMoved += pointComponent.OnEnemyMove;
        pointComponent.captured.AddListener(() => OnPointCaptured(pointComponent));
    }

    public void OnPointCaptured(CapturePoint capturePoint) {
        CapturePointPositions.Remove(capturePoint.GridPosition);
        enemyHandler.enemiesMoved -= capturePoint.OnEnemyMove;

        if (CapturePointPositions.Count == 0) {
            allPointsCaptured.Invoke();
        }
    }
}