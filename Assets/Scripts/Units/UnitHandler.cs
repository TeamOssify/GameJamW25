using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.Tilemaps;

public class UnitHandler : MonoBehaviour {
    [SerializeField]
    private Tilemap tileMap;

    public UnitComponent pawn;
    public UnitComponent knight;
    public UnitComponent king;
    public UnitComponent barbarian;
    public UnitComponent jarl;

    public LongUnitComponent rook;
    public LongUnitComponent bishop;
    public LongUnitComponent queen;


    private TileComponent _tileComponent;

    private readonly Dictionary<Vector3Int, UnitComponent> _unitGridPositions = new();

    private UnitComponent _selectedUnit;
    private readonly HashSet<Vector3Int> _selectedUnitMoves = new();

    private void Start() {
        _tileComponent = tileMap.GetComponent<TileComponent>();

        SpawnUnit(new Vector3Int(-5,1,0), pawn);
        SpawnUnit(new Vector3Int(-1, 1,0), rook);
        SpawnUnit(new Vector3Int(-3,1,0), bishop);
        SpawnUnit(new Vector3Int(0,1,0), knight);
        SpawnUnit(new Vector3Int(-3,3,0), queen);
        SpawnUnit(new Vector3Int(4,-3,0), king);
        SpawnUnit(new Vector3Int(4,-2,0), barbarian);
        SpawnUnit(new Vector3Int(4,-1,0), jarl);

    }

    public void SpawnUnit(Vector3Int gridPos, UnitComponent unitType) {
        if (!_tileComponent.IsUnobstructedTile(gridPos)) {
            Debug.LogError($"Invalid grid position: {gridPos}");
        }
        UnitComponent newUnit = Instantiate(unitType, Vector3.zero, Quaternion.identity, gameObject.transform);
        _tileComponent.TryGetWorldPositionForTileCenter(gridPos, out var pos);
        newUnit.Move(pos,gridPos, true);
        _unitGridPositions.Add(gridPos, newUnit);
    }
    private void SelectUnit(UnitComponent unit) {
        if (_selectedUnit) {
            DeselectUnit();
        }

        _selectedUnit = unit;
        _selectedUnit.Select();

        var unitMoves = _selectedUnit.GetUnitMoves(_tileComponent);
        unitMoves.RemoveAll(x => !_tileComponent.IsUnobstructedTile(x));

        _selectedUnitMoves.AddRange(unitMoves);
        _tileComponent.SetTileHints(unitMoves);
    }

    private void DeselectUnit() {
        _selectedUnit.Deselect();
        _selectedUnitMoves.Clear();
        _tileComponent.ClearTileHints();
        _selectedUnit = null;
    }

    public void SelectTile(Vector3Int gridPosition) {
        if (!_tileComponent.IsUnobstructedTile(gridPosition)) {
            // Blocked tile
            DeselectUnit();
            return;
        }

        if (TryGetUnitAtGridPosition(gridPosition, out var unit)) {
            // Clicked a unit
            SelectUnit(unit);
            return;
        }

        if (!_selectedUnit) {
            // No unit selected
            return;
        }

        if (_selectedUnitMoves.Contains(gridPosition)
            && _tileComponent.TryGetWorldPositionForTileCenter(gridPosition, out var worldPos)
        ) {
            // Unit selected, clicked valid move tile
            _unitGridPositions.Remove(_selectedUnit.GridPos);
            _selectedUnit.Move(worldPos, gridPosition);
            _unitGridPositions.Add(gridPosition, _selectedUnit);
        }

        DeselectUnit();
    }

    public bool TryGetUnitAtGridPosition(Vector3Int gridPosition, out UnitComponent unit) {
        return _unitGridPositions.TryGetValue(gridPosition, out unit);
    }
}