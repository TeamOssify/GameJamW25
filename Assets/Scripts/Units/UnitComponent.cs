using System;
using UnityEngine;

public class UnitComponent : MonoBehaviour {
    public string UnitName => unitName;

    [SerializeField]
    private string unitName;

    public string UnitDescription => unitDescription;

    [SerializeField]
    private string unitDescription;

    public Sprite UnitSprite => unitSprite;

    [SerializeField]
    private Sprite unitSprite;

    public int RespawnCost => unitRespawnCost;

    [SerializeField]
    private int unitRespawnCost;

    public int UpgradeCost => unitUpgradeCost;

    [SerializeField]
    private int unitUpgradeCost;

    [SerializeField]
    private MovementComponentBase unitBaseMoves;

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

        if (moveSet.NormalMoves.Count == 0 && moveSet.JumpMoves.Count == 0) {
            Debug.LogWarning($"Unit {unitName} has no known moves! (Including invalid)");
        }

        moveSet.NormalMoves.RemoveAll(normalAdditionalFilter);
        moveSet.JumpMoves.RemoveAll(jumpAdditionalFilter);

        var notVisited = Bfs.FloodFill(GridPos, moveSet.NormalMoves, tileComponent);
        moveSet.NormalMoves.RemoveAll(x => notVisited.Contains(x));

        return moveSet;
    }

    // Getter for _hasMoved
    // Probably messier than having a HashMap in UnitHandler but whatev
    // UPDATE so this doesnt work at all
    // Leaving this here cuz I think its funny :p - Takk
    public bool GetMoved() {
        if (this._hasMoved) {
            return true;
        }

        return false;
    }
}