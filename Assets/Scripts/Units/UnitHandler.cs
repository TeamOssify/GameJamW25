using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class UnitHandler : MonoBehaviour {
    [SerializeField]
    private Tilemap tileMap;
    private TileComponent _tileComponent;

    private readonly Dictionary<Vector3Int, UnitComponent> _unitGridPositions = new();
    private UnitComponent _selectedUnit;
    
    private void Start() {
        _tileComponent = tileMap.GetComponent<TileComponent>();
    }

    private void SelectUnit(UnitComponent unit) {
        if (_selectedUnit) {
            _selectedUnit.Deselect();
        }
        
        _selectedUnit = unit;
        _selectedUnit.Select();
    }

    public void SelectTile(Vector3Int gridPosition) {
        UnitComponent unit = GetUnitAtGridPosition(gridPosition);
        if (!_selectedUnit) {
            if (unit) {
                SelectUnit(unit);
            }
            return;
        }
        
        if (_selectedUnit.IsValidMove(gridPosition)) {
            _selectedUnit = null;
            _unitGridPositions.Remove(unit.Position);
            unit.Move(gridPosition);
            _unitGridPositions.Add(gridPosition, unit);
        }
        else if (unit) {
            SelectUnit(unit);
        }
        else {
            _selectedUnit.Deselect();
            _selectedUnit = null;
        }
    }

    public UnitComponent GetUnitAtGridPosition(Vector3Int gridPosition) {
        _unitGridPositions.TryGetValue(gridPosition, out var unit);
        return unit;
    }
    
    public Vector3 GetWorldPositionFromGrid(Vector3Int gridPosition) {
        if (!_tileComponent.TryGetWorldPositionForTile(gridPosition, out var worldPos)) {
            Debug.Log($"Grid position {gridPosition} is invalid.");
            return Vector3.zero;
        }

        return worldPos;
    }
}
