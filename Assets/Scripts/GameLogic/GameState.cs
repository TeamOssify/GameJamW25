using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// Combines the functionality CapturePointHandler and UnitHandler
public class GameState : MonoBehaviour
{
    // Prefabs
    [SerializeField]
    private GameObject capturePointHandlerPrefab;

    // References
    [SerializeField]
    private Tilemap tilemap;

    [SerializeField]
    private EnemyHandler enemyHandler;
    [SerializeField]
    private UnitHandler unitHandler;

    // Setup
    [SerializeField]
    private List<UnitPosition> unitPositions;
    [SerializeField]
    private List<Vector3Int> capturePointPositions;

    private EnemyHandler _enemyHandler;
    private CapturePointHandler _capturePointHandler;
    private TileComponent _tileComponent;

    public bool HandlersReady { get; private set; } = false;

    private void Start() {
        _capturePointHandler = Instantiate(capturePointHandlerPrefab, transform).GetComponent<CapturePointHandler>();
        _tileComponent = tilemap.GetComponent<TileComponent>();

        // Inject required fields
        _capturePointHandler.tileComponent = _tileComponent;
        _capturePointHandler.enemyHandler = enemyHandler;
        unitHandler.capturePointHandler = _capturePointHandler;

        // Create Capture points and units
        _capturePointHandler.captureSpawns = capturePointPositions;
        _capturePointHandler.SpawnCapturePoints();

        _capturePointHandler.allPointsCaptured.AddListener(DisableInput);
        HandlersReady = true;
    }

    private void OnDestroy() {
        _capturePointHandler.allPointsCaptured.RemoveListener(DisableInput);
    }

    private void DisableInput() {
        _tileComponent.Interactable = false;
    }

    private void EnableInput() {
        _tileComponent.Interactable = true;
    }

    // Had to do this bc System.Tuple isn't serializable
    [Serializable]
    private class UnitPosition {
        public UnitComponent pieceType; 
        public Vector3Int position;
    }
}
