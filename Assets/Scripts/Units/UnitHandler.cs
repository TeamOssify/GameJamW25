using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Unity.VisualScripting;
using UnityEngine.Tilemaps;

public class UnitHandler : MonoBehaviour {
    [SerializeField]
    private Tilemap tileMap;

    public UnitComponent testUnit;

    private TileComponent _tileComponent;

    private readonly Dictionary<Vector3Int, UnitComponent> _unitGridPositions = new();

    private UnitComponent _selectedUnit;
    private readonly HashSet<Vector3Int> _selectedUnitMoves = new();

    private void Start() {
        _tileComponent = tileMap.GetComponent<TileComponent>();

        UnitComponent littleGuy = Instantiate(testUnit, Vector3.zero, Quaternion.identity);
        littleGuy.Move(new Vector3(-1.5f, -1.5f, 0), new Vector3Int(-2, -2, 0));
        if (_tileComponent.TryGetTileForWorldPosition(littleGuy.GridPos, out var pos)) {
            _unitGridPositions.Add(pos, littleGuy);
        }

        UnitComponent bigGuy = Instantiate(testUnit, Vector3.zero, Quaternion.identity);
        bigGuy.Move(new Vector3(-3.5f,1.5f,0), new Vector3Int(-4,1,0));
        if (_tileComponent.TryGetTileForWorldPosition(bigGuy.GridPos, out var bpos)) {
            _unitGridPositions.Add(bpos, bigGuy);
        }
    }

    private void SelectUnit(UnitComponent unit) {
        if (_selectedUnit) {
            DeselectUnit();
        }

        _selectedUnit = unit;
        _selectedUnit.Select();

        var unitMoves = _selectedUnit.GetUnitMoves();
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
        if (!_tileComponent.IsValidTile(gridPosition)) {
            return;
        }

        if (!_selectedUnit) {
            if (TryGetUnitAtGridPosition(gridPosition, out var unit)) {
                SelectUnit(unit);
            }

            return;
        }

        if (!IsOccupiedTile(gridPosition)
            && _selectedUnitMoves.Contains(gridPosition)
            && _tileComponent.TryGetWorldPositionForTileCenter(gridPosition, out var worldPos)
        ) {
            _unitGridPositions.Remove(_selectedUnit.GridPos);
            _selectedUnit.Move(worldPos, gridPosition);
            _unitGridPositions.Add(gridPosition, _selectedUnit);
        }

        DeselectUnit();
    }

    private bool IsOccupiedTile(Vector3Int gridPosition) {
        return _unitGridPositions.ContainsKey(gridPosition);
    }

    public bool TryGetUnitAtGridPosition(Vector3Int gridPosition, out UnitComponent unit) {
        return _unitGridPositions.TryGetValue(gridPosition, out unit);
    }
}