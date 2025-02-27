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
    private GameObject unitHandlerPrefab;
    [SerializeField]
    private GameObject capturePointHandlerPrefab;

    // References
    [SerializeField]
    private Tilemap tilemap;
    [SerializeField]
    private DeployAreaComponent deployArea;
    [SerializeField]
    private GameObject unitFrame;

    // Setup
    [SerializeField]
    private List<UnitPosition> unitPositions;
    [SerializeField]
    private List<Vector3Int> capturePointPositions;

    private UnitHandler _unitHandler;
    private CapturePointHandler _capturePointHandler;
    private TileComponent _tileComponent;

    public bool HandlersReady { get; private set; } = false;

    private IEnumerator Start() {
        _unitHandler = Instantiate(unitHandlerPrefab, transform).GetComponent<UnitHandler>();
        _capturePointHandler = Instantiate(capturePointHandlerPrefab, transform).GetComponent<CapturePointHandler>();
        _tileComponent = tilemap.GetComponent<TileComponent>();

        // Inject required fields
        _unitHandler.tileMap = tilemap;
        _unitHandler.deployArea = deployArea;
        _unitHandler.unitInterface = unitFrame;
        _capturePointHandler.tilemap = tilemap;
        _capturePointHandler.unitHandler = _unitHandler;

        // Create Capture points and units
        _capturePointHandler.spawnPoints = capturePointPositions;
        yield return new WaitUntil(() => _unitHandler.isReady); // In order ot prevent a null error caused by _tileComponent in unitHandler
        foreach (var i in unitPositions) {
            _unitHandler.SpawnUnit(i.position, i.pieceType);
        }

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
