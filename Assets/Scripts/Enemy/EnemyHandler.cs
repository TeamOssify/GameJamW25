using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandler : MonoBehaviour {
    [SerializeField]
    private TileComponent tileComponent;

    [SerializeField]
    private UnitHandler unitHandler;

    [SerializeField]
    private GameObject portalContainer;

    private Vector3Int[] _pointsOfInterest;

    private Dictionary<Vector3Int, EnemyComponent> _enemyGridPositions = new();
    private Dictionary<Vector3Int, EnemyComponent> _futureEnemyGridPositions = new();
    private Dictionary<Vector3Int, int> _enemyPortalPositions = new();

    public EventHandler<Dictionary<Vector3Int, EnemyComponent>> enemiesMoved;
    public EnemyPortalComponent portalPrefab;

    public bool CanSpawnEnemy(Vector3Int gridPos) {
        return !unitHandler.TryGetUnitAtGridPosition(gridPos, out _) && !_enemyGridPositions.ContainsKey(gridPos);
    }

    public void SpawnEnemy(EnemyComponent enemy, Vector3Int gridPos) {
        // if (!tileComponent.IsUnobstructedTile(gridPos)) {
        //     Debug.LogError($"Invalid grid position: {gridPos}");
        //     return;
        // }

        if (unitHandler.TryGetUnitAtGridPosition(gridPos, out _)) {
            Debug.LogWarning($"Spawning enemy on top of unit at {gridPos}!");
        }

        var newUnit = Instantiate(enemy, Vector3.zero, Quaternion.identity, gameObject.transform);
        if (!tileComponent.TryGetWorldPositionForTileCenter(gridPos, out var pos)) {
            Debug.LogError($"Failed to find tile center for {gridPos}!");
            return;
        }   

        newUnit.Move(pos,gridPos);
        _enemyGridPositions.Add(gridPos, newUnit);
    }

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
        enemiesMoved?.Invoke(this, _enemyGridPositions);
    }

    public bool WouldCaptureEnemy(Vector3Int gridPos) {
        return _enemyGridPositions.ContainsKey(gridPos);
    }

    public void InitWorldPortals() {
        foreach (EnemyPortalComponent portal in portalContainer.GetComponentsInChildren<EnemyPortalComponent>()) {
            tileComponent.TryGetTileForWorldPosition(portal.transform.position, out var gridPos);
            AddPortal(gridPos,portal);
        }
    }

    public void AddPortal(Vector3Int initPos, EnemyPortalComponent newPortal) {
        newPortal.SpawnPortal(initPos);
        _enemyPortalPositions.Add(initPos, _enemyPortalPositions.Count);
    }

    public void TriggerWorldPortals() {
        foreach (EnemyPortalComponent portal in portalContainer.GetComponentsInChildren<EnemyPortalComponent>()) {
            StepPortal(portal);
        }
    }
    public void StepPortal(EnemyPortalComponent portal) {
        foreach (Wave wave in portal.GetComponentsInChildren<Wave>()) {
            if (portal.currentWave == wave.WaveNumber) {
                SpawnWave(wave, portal);
                portal.currentWave++;
                return;
            }
        }
    }

    public void SpawnWave(Wave wave, EnemyPortalComponent portal) {
        foreach (SubWave subWave in wave.SubWaves) {
            SpawnSubwave(subWave, portal);
        }
    }

    public void SpawnSubwave(SubWave wave, EnemyPortalComponent portal) {

        for (int i = 0; i < wave.EnemyCount; i++) {
            SpawnEnemy(wave.EnemyToSpawn, portal.GridPos);
        }

        ComputeEnemyMoves();
        MoveEnemies();
    }
}