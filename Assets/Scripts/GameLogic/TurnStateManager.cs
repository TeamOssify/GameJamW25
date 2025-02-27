using UnityEngine;
using System;



public class TurnStateManager : MonoBehaviour {
    [SerializeField]
    private UnitHandler unitHandler;

    [SerializeField]
    private MainHudController mainHudController;

    public enum TurnState {
        Player,
        Enemy
    };

    public int TurnCount {get; private set;}
    public TurnState CurrentTurnState { get; private set; }
    private bool _firstPlayerTurn = true;

    private void Start() {
        CurrentTurnState = TurnState.Player;
    }

    public void EndPlayerTurn() {
        if (CurrentTurnState == TurnState.Player) {
            CurrentTurnState = TurnState.Enemy;
        }
        _firstPlayerTurn = false;
        mainHudController.UpdateTurnCount(TurnCount + 1);
        BeginEnemyTurn();

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
            unitHandler.DeployMode = true;
        }
    }

    public void BeginEnemyTurn() {

    }
}
