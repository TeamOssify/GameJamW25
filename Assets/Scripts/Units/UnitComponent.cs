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

    public MoveSet GetUnitMoves(TileComponent tileComponent, Predicate<Vector3Int> normalAdditionalFilter, Predicate<Vector3Int> jumpAdditionalFilter) {
        var moveSet = new MoveSet();

        unitBaseMoves.GetMoves(moveSet, GridPos, tileComponent);

        if (!_hasMoved && unitFirstMoves) {
            unitFirstMoves.GetMoves(moveSet, GridPos, tileComponent);
        }

        if (unitTier1Moves) {
            unitTier1Moves.GetMoves(moveSet, GridPos, tileComponent);
        }

        moveSet.NormalMoves.RemoveAll(normalAdditionalFilter);
        moveSet.JumpMoves.RemoveAll(jumpAdditionalFilter);

        var notVisited = Bfs.FloodFill(GridPos, moveSet.NormalMoves, tileComponent);
        moveSet.NormalMoves.RemoveAll(x => notVisited.Contains(x));

        return moveSet;
    }
}