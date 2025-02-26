using System;
using UnityEngine;

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
    protected MovementComponentBase unitBaseMoves;

    [SerializeField]
    private MovementComponentBase unitFirstMoves;

    [SerializeField]
    private MovementComponentBase unitTier1Moves;

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

    public SwapBackArray<(Vector3Int pos, MoveType type)> GetUnitMoves(TileComponent tileComponent, Predicate<(Vector3Int pos, MoveType type)> additionalFilter) {
        var moves = new SwapBackArray<(Vector3Int pos, MoveType type)>();

        unitBaseMoves.GetMoves(moves, GridPos, tileComponent);

        if (!_hasMoved && unitFirstMoves) {
            unitFirstMoves.GetMoves(moves, GridPos, tileComponent);
        }

        if (unitTier1Moves) {
            unitTier1Moves.GetMoves(moves, GridPos, tileComponent);
        }

        moves.RemoveAll(additionalFilter);

        Bfs.FilterMoves(moves, tileComponent);

        return moves;
    }
}