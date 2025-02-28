using UnityEngine;
using System;
using System.Collections.Generic;

public class TurnStateManager : MonoBehaviour {
    [SerializeField]
    private UnitHandler unitHandler;

    [SerializeField]
    private MainHudController mainHudController;

    [SerializeField]
    private DeployAreaComponent deployArea;

    [SerializeField]
    private EnemyHandler enemyHandler;

    [SerializeField]
    private TileComponent tileComponent;

    public enum TurnState {
        Player,
        Enemy,
        Deployment
    };

    public int TurnCount = 1;
    public TurnState CurrentTurnState { get; private set; }
    private bool _firstPlayerTurn = true;


    private int _unitsDeployed = 0;
    private readonly Queue<UnitComponent> _unitsToBeDeployed = new();
    private bool _waitingDeployment;

    public EventHandler OnNextPlayerTurn;

    private void Start() {
        CurrentTurnState = TurnState.Player;
        enemyHandler.InitWorldPortals();
        BeginPlayerTurn();
    }

    public void EndPlayerTurn() {
        if (CurrentTurnState == TurnState.Player) {
            CurrentTurnState = TurnState.Enemy;

            _firstPlayerTurn = false;
            TurnCount++;
            Debug.Log("Player Turn: " + CurrentTurnState + " Count: " + TurnCount);
            mainHudController.UpdateTurnCount(TurnCount);
            BeginEnemyTurn();
        }
    }

    public void EndEnemyTurn() {
        if (CurrentTurnState == TurnState.Enemy) {
            CurrentTurnState = TurnState.Player;
        }
        mainHudController.UpdateTurnCount(TurnCount);
        BeginPlayerTurn();
    }

    public void BeginPlayerTurn() {
        OnNextPlayerTurn?.Invoke(this, EventArgs.Empty);

        if (_firstPlayerTurn) {
            unitHandler.InitDeploy();
        }
    }

    public void BeginEnemyTurn() {
        enemyHandler.MoveEnemies();
        tileComponent.ClearTileHints(HintBucket.EnemyTelegraph);
        enemyHandler.TriggerWorldPortals();
        enemyHandler.ComputeEnemyMoves();
        EndEnemyTurn();
    }

    public void EnterDeployment() {
        Debug.Log("Entering deployment");
        CurrentTurnState = TurnState.Deployment;
        deployArea.ToggleDeployAreaVisibility();

        _unitsToBeDeployed.Clear();
        foreach (var unit in unitHandler.equippedUnits) {
            _unitsToBeDeployed.Enqueue(unit);
        }

        StartNextUnitDeployment();
    }

    private void StartNextUnitDeployment() {
        Debug.Log(_unitsToBeDeployed.Count);
        if (_unitsToBeDeployed.Count > 0) {
            Debug.Log("Next deployment");

            UnitComponent nextUnit = _unitsToBeDeployed.Dequeue();
            unitHandler.SetUnitForDeployment(nextUnit);
            unitHandler.DeployMode = true;
            _waitingDeployment = true;
        }
        else if (_unitsToBeDeployed.Count == 0 && !_waitingDeployment) {
            FinishDeployment();
        }
    }

    public void UnitDeployed() {
        Debug.Log("Unit deployed");

        _unitsDeployed++;
        _waitingDeployment = false;

        if (_unitsToBeDeployed.Count > 0) {
            StartNextUnitDeployment();
        }
        else {
            FinishDeployment();
        }
    }

    private void FinishDeployment() {
        Debug.Log("Finish deployment");

        deployArea.ToggleDeployAreaVisibility();
        unitHandler.DeployMode = false;
        unitHandler.SetUnitForDeployment(null);
        CurrentTurnState = TurnState.Player;
    }

    //TODO: add respawning stuff
}
