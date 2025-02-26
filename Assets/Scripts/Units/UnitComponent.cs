using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class UnitComponent : MonoBehaviour {
    [SerializeField]
    private string unitName;

    [SerializeField]
    private string unitDescription;

    [SerializeField]
    public Sprite unitSprite;

    [SerializeField]
    private int unitRespawnCost;

    [SerializeField]
    private int unitUpgradeCost;

    [SerializeField]
    protected Tilemap unitBaseMoves;

    [SerializeField]
    private Tilemap unitFirstMoves;

    [SerializeField]
    private Tilemap unitTier1Moves;

    public Vector3Int GridPos { get; private set; }
    public int currentTier = 1;

    private bool _hasMoved;
    public void Select() {
        Debug.Log("Selected a unit");
    }

    public void Deselect() {
        Debug.Log("Deselected a unit");
    }

    public void Move(Vector3 pos, Vector3Int gridPosition, bool initializing = false) {
        transform.position = pos;
        GridPos = gridPosition;
        if (!initializing) {
            _hasMoved = true;
        }
    }

    public SwapBackArray<Vector3Int> GetUnitMoves(TileComponent tileComponent) {
        var moves = GetMoves(tileComponent, unitBaseMoves);
        if (!_hasMoved && unitFirstMoves) {
            var firstMoves = GetMoves(tileComponent, unitFirstMoves);
            moves.AddRange(firstMoves);
        }

        if (unitTier1Moves) {
            var upgradeMoves = GetMoves(tileComponent, unitTier1Moves);
            moves.AddRange(upgradeMoves);
        }

        return moves;
    }

    protected virtual SwapBackArray<Vector3Int> GetMoves(TileComponent tileComponent, Tilemap moveMap) {
        var movesSize = moveMap.size;
        var movesOrigin = moveMap.origin;

        var moves = new SwapBackArray<Vector3Int>();
        for (var y = 0; y < movesSize.y; y++)
        for (var x = 0; x < movesSize.x; x++) {
            var tilePos = new Vector3Int(x, y) + movesOrigin;

            if (tilePos is { x: 0, y: 0 }) {
                // Don't display current unit tile as a move
                continue;
            }

            if (moveMap.HasTile(tilePos)) {
                moves.Add(tilePos + GridPos);
            }
        }
        
        return moves;
    }
}