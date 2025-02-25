using System;
using System.Collections.Generic;
using UnityEngine;

public enum UnitType {Pawn, Knight, Bishop, Rook, Queen, King, Barbarian, Duke}

public class UnitComponent : MonoBehaviour {
    public UnitType Type;
    public Vector2Int Position;

    private bool HasMoved = false;
    

    public void Select() {
        DisplayValidMoves();
    }

    public void Deselect() {
        
    }

    public void Move() {
        
    }

    private void GetValidMoves() {
        
    }

    private void DisplayValidMoves() {
        
    }

    private void ClearValidMoves() {
        
    }

    void Start() {
        
    }
}