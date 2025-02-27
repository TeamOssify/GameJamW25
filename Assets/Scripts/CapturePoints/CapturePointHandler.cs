using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CapturePointHandler : MonoBehaviour {
    [SerializeField]
    private GameObject capturePointPrefab;

    private TileComponent tileComponent;

    private int capturePoints = 0;

    public Tilemap tilemap;
    public UnitHandler unitHandler;
    public List<Vector3Int> spawnPoints;

    public void Start() {
        tileComponent = tilemap.GetComponent<TileComponent>();
        foreach (var i in spawnPoints) {
            AddCapturePoint(i);
        }
    }

    public void AddCapturePoint(Vector3Int i) {
        var newPoint = Instantiate(capturePointPrefab, transform);
        var pointComponent = newPoint.GetComponent<CapturePoint>();

        tileComponent.TryGetWorldPositionForTileCenter(i, out var pos);
        pointComponent.Move(pos, i);

        capturePoints++;

        unitHandler.unitMoved += pointComponent.onUnitMove;
        pointComponent.captured.AddListener(() => {onPointCaptured(pointComponent);});
    }

    public void onPointCaptured(CapturePoint capturePoint) {
        capturePoints--;
        unitHandler.unitMoved -= capturePoint.onUnitMove;

        if (capturePoints <= 0) {
            Debug.Log("All points have been captured");
        }
    }
}