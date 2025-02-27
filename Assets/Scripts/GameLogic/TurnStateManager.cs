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

    public enum TurnState {
        Player,
        Enemy,
        Deployment
    };

    public int TurnCount = 1;
    public TurnState CurrentTurnState { get; private set; }
    private bool _firstPlayerTurn = true;

    private int _unitsDeployed = 0;
    private Queue<UnitComponent> _unitsToBeDeployed = new Queue<UnitComponent>();
    private bool _waitingDeployment;

    private void Start() {
        CurrentTurnState = TurnState.Player;
        BeginPlayerTurn();
    }

    public void EndPlayerTurn() {
        if (CurrentTurnState == TurnState.Player) {
            CurrentTurnState = TurnState.Enemy;

            _firstPlayerTurn = false;
            //mainHudController.UpdateTurnCount(TurnCount + 1);
            TurnCount++;
            Debug.Log("Player Turn: " + CurrentTurnState + " Count: " + TurnCount);
            BeginEnemyTurn();
        }
    }

    public void EndEnemyTurn() {
        if (CurrentTurnState == TurnState.Enemy) {
            CurrentTurnState = TurnState.Player;
        }
        mainHudController.UpdateTurnCount(TurnCount + 1);
        BeginPlayerTurn();
    }

    public void BeginPlayerTurn() {
        if (_firstPlayerTurn) {
            unitHandler.InitDeploy();
        }
    }

    public void BeginEnemyTurn() {

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
