using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Unity.VisualScripting;
using UnityEngine.Tilemaps;

public class UnitHandler : MonoBehaviour {
    [SerializeField]
    private Tilemap tileMap;
    private TileComponent _tileComponent;

    private readonly Dictionary<Vector3Int, UnitComponent> _unitGridPositions = new();

    private UnitComponent _selectedUnit;
    private readonly HashSet<Vector3Int> _selectedUnitMoves = new();

    private void Start() {
        _tileComponent = tileMap.GetComponent<TileComponent>();
    }

    private void SelectUnit(UnitComponent unit) {
        if (_selectedUnit) {
            DeselectUnit();
        }

        _selectedUnit = unit;
        _selectedUnit.Select();
        _selectedUnitMoves.AddRange(_selectedUnit.GetUnitMoves());
    }

    private void DeselectUnit() {
        _selectedUnit.Deselect();
        _selectedUnitMoves.Clear();
        _selectedUnit = null;
    }

    public void SelectTile(Vector3Int gridPosition) {
        Debug.Log("yo zora");
        if (!_tileComponent.IsValidTile(gridPosition)) {
            return;
        }

        var unit = GetUnitAtGridPosition(gridPosition);
        if (!_selectedUnit) {
            if (unit) {
                SelectUnit(unit);
            }

            return;
        }

        if (_selectedUnitMoves.Contains(gridPosition)) {
            if (_tileComponent.IsValidTile(gridPosition)) {
                _unitGridPositions.Remove(unit!.Position);
                unit.Move(GetWorldPositionFromGrid(gridPosition));
                _unitGridPositions.Add(gridPosition, unit);
            }

            DeselectUnit();
        }
        else if (unit) {
            SelectUnit(unit);
        }
        else {
            DeselectUnit();
        }
    }

    [return: MaybeNull]
    public UnitComponent GetUnitAtGridPosition(Vector3Int gridPosition) {
        return _unitGridPositions.GetValueOrDefault(gridPosition);
    }

    public Vector3 GetWorldPositionFromGrid(Vector3Int gridPosition) {
        if (!_tileComponent.TryGetWorldPositionForTile(gridPosition, out var worldPos)) {
            Debug.Log($"Grid position {gridPosition} is invalid.");
            return Vector3.zero;
        }

        return worldPos;
    }
}