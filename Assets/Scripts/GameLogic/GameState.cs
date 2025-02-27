using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

    private UnitHandler unitHandler;
    private CapturePointHandler capturePointHandler;

    public bool handlersReady {get; private set;} = false;
    IEnumerator Start()
    {
        unitHandler = Instantiate(unitHandlerPrefab, transform).GetComponent<UnitHandler>();
        capturePointHandler = Instantiate(capturePointHandlerPrefab, transform).GetComponent<CapturePointHandler>();

        // Inject required fields
        unitHandler.tileMap = tilemap;
        unitHandler.deployArea = deployArea;
        unitHandler.unitInterface = unitFrame;
        capturePointHandler.tilemap = tilemap;
        capturePointHandler.unitHandler = unitHandler;

        // Create Capture points and units
        capturePointHandler.spawnPoints = capturePointPositions;
        yield return new WaitUntil(() => unitHandler.isReady); // In order ot prevent a null error caused by _tileComponent in unitHandler
        foreach (var i in unitPositions) {
            unitHandler.SpawnUnit(i.position, i.pieceType);
        }

        capturePointHandler.allPointsCaptured.AddListener(disableInput);
        handlersReady = true;
    }


    private void disableInput() {
        Destroy(unitHandler);
    }

    // Had to do this bc System.Tuple isn't serializable
    [Serializable]
    private class UnitPosition {
        public UnitComponent pieceType; 
        public Vector3Int position;
    }
}
