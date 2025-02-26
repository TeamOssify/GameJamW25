using UnityEngine;
using System;



public class TurnStateManager : MonoBehaviour {
    [SerializeField]
    private UnitHandler unitHandler;
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
        BeginEnemyTurn();
    }

    public void EndEnemyTurn() {
        if (CurrentTurnState == TurnState.Enemy) {
            CurrentTurnState = TurnState.Player;
        }
        BeginPlayerTurn();
    }

    public void BeginPlayerTurn() {
        if (_firstPlayerTurn) {
        }
    }

    public void BeginEnemyTurn() {

    }
}
