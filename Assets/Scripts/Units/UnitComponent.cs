using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum UnitType {Pawn, Knight, Bishop, Rook, Queen, King, Barbarian, Duke}

public class UnitComponent : MonoBehaviour {
    public Vector3Int Position { get; private set; }

    [SerializeField]
    private UnitHandler unitHandler;

    [SerializeField]
    private Tilemap unitBaseMoves;

    public void Select() { }

    public void Deselect() { }

    public bool IsValidMove(Vector3Int gridPosition) {
        Debug.Log("Yo");
        return false;
    }

    public void Move(Vector3Int gridPosition) {
        //if the move is valid (do later lule)
        transform.position = unitHandler.GetWorldPositionFromGrid(gridPosition);
    }

    public Vector3Int[] GetUnitMoves() {
        var moves = GetBaseMoves();

        var upgradeMoves = GetUpgradeMoves();
        moves.AddRange(upgradeMoves);

        return moves.ToArray();
    }

    private List<Vector3Int> GetBaseMoves() {
        var movesSize = unitBaseMoves.size;
        var movesOrigin = unitBaseMoves.origin;

        var moves = new List<Vector3Int>();
        for (var y = 0; y < movesSize.y; y++)
        for (var x = 0; x < movesSize.x; x++) {
            var tilePos = new Vector3Int(x, y) - movesOrigin;
            var tile = unitBaseMoves.GetTile(tilePos);
            if (tile) {
                moves.Add(tilePos);
            }
        }
        
        return moves;
    }

    private Vector3Int[] GetUpgradeMoves() {
        return Array.Empty<Vector3Int>();
    }
}