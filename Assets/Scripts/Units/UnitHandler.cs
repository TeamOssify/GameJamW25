using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
public class UnitHandler : MonoBehaviour
{
    [SerializeField] private Tilemap tileMap;
    
    private Dictionary<Vector3Int, UnitComponent> _unitGridPositions = new Dictionary<Vector3Int, UnitComponent>();
    private UnitComponent _selectedUnit;
    
    
    private void Start() {
        
    }
    private void SelectUnit(UnitComponent unit) {
        if (_selectedUnit != null) {
            _selectedUnit.Deselect();
        }
        
        _selectedUnit = unit;
        _selectedUnit.Select();
    }

    public void SelectTile(Vector3Int gridPosition) {
        UnitComponent unit = GetUnitAtGridPosition(gridPosition);
        if (_selectedUnit == null) {
            if (unit) {
                SelectUnit(unit);
            }
            return;
        }
        
        if (_selectedUnit.IsValidMove(gridPosition)) {
            _selectedUnit = null;
            _unitGridPositions.Remove(unit.position);
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
        _unitGridPositions.TryGetValue(gridPosition, out UnitComponent unit);
        return unit;
    }
    
    public Vector3 GetWorldPositionFromGrid(Vector3Int gridPosition) {
        // Convert grid position to world position
        Vector3Int cellPosition = new Vector3Int(gridPosition.x, gridPosition.y, 0);
        return tileMap.GetCellCenterWorld(cellPosition);
    }
}
