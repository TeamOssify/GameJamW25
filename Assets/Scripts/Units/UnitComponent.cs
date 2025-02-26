using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum UnitType {Pawn, Knight, Bishop, Rook, Queen, King, Barbarian, Duke}

public class UnitComponent : MonoBehaviour {
    [SerializeField]
    private string unitName;

    [SerializeField]
    private string unitDescription;

    [SerializeField]
    private Tilemap unitBaseMoves;

    public Vector3Int GridPos { get; private set; }

    public void Select() {
        Debug.Log("Selected a unit");
    }

    public void Deselect() {
        Debug.Log("Deselected a unit");
    }

    public void Move(Vector3 pos, Vector3Int gridPosition) {
        transform.position = pos;
        GridPos = gridPosition;
        Debug.Log("tried move to world pos " + pos + " grid pos: " + gridPosition);
    }

    public SwapBackArray<Vector3Int> GetUnitMoves() {
        var moves = GetBaseMoves();

        var upgradeMoves = GetUpgradeMoves();
        moves.AddRange(upgradeMoves);

        return moves;
    }

    private SwapBackArray<Vector3Int> GetBaseMoves() {
        var movesSize = unitBaseMoves.size;
        var movesOrigin = unitBaseMoves.origin;

        var moves = new SwapBackArray<Vector3Int>();
        for (var y = 0; y < movesSize.y; y++)
        for (var x = 0; x < movesSize.x; x++) {
            var tilePos = new Vector3Int(x, y) + movesOrigin;

            if (tilePos is { x: 0, y: 0 }) {
                // Don't display current unit tile as a move
                continue;
            }

            if (unitBaseMoves.HasTile(tilePos)) {
                moves.Add(tilePos + GridPos);
            }
        }
        
        return moves;
    }

    private Vector3Int[] GetUpgradeMoves() {
        return Array.Empty<Vector3Int>();
    }
    
    
}