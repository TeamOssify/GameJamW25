using System;
using System.Collections.Generic;
using UnityEngine;

public enum UnitType {Pawn, Knight, Bishop, Rook, Queen, King, Barbarian, Duke}

public class UnitComponent : MonoBehaviour {
    public UnitType Type;
    public bool HasMoved = false;
    public Vector2Int Position;

    void Start() {
        
    }
}