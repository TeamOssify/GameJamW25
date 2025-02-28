using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class CapturePointHandler : MonoBehaviour {
    [SerializeField]
    private GameObject capturePointPrefab;

    private TileComponent _tileComponent;

    public readonly HashSet<Vector3Int> CapturePointPositions = new();

    public Tilemap tilemap;
    public EnemyHandler enemyHandler;
    public List<Vector3Int> spawnPoints;

    public UnityEvent allPointsCaptured;

    public void Start() {
        _tileComponent = tilemap.GetComponent<TileComponent>();
        foreach (var i in spawnPoints) {
            AddCapturePoint(i);
        }
    }

    public void AddCapturePoint(Vector3Int i) {
        var newPoint = Instantiate(capturePointPrefab, transform);
        var pointComponent = newPoint.GetComponent<CapturePoint>();

        _tileComponent.TryGetWorldPositionForTileCenter(i, out var pos);
        pointComponent.Move(pos, i);

        CapturePointPositions.Add(i);

        enemyHandler.enemiesMoved += pointComponent.OnEnemyMove;
        pointComponent.captured.AddListener(() => {OnPointCaptured(pointComponent);});
    }

    public void OnPointCaptured(CapturePoint capturePoint) {
        CapturePointPositions.Remove(capturePoint.GridPosition);
        enemyHandler.enemiesMoved -= capturePoint.OnEnemyMove;

        if (CapturePointPositions.Count == 0) {
            allPointsCaptured.Invoke();
        }
    }
}