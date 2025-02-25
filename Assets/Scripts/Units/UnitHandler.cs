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
        littleGuy.Move(new Vector3(-1.5f,-1.5f,0), new Vector3Int(-2,-2,0));
        if (_tileComponent.TryGetTileForWorldPosition(littleGuy.Position, out var pos)) {
            _unitGridPositions.Add(pos, littleGuy);
        }
        
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
        if (!_tileComponent.IsValidTile(gridPosition)) {
            return;
        }
        //if clicked tile is valid
        var unit = GetUnitAtGridPosition(gridPosition);
        if (!_selectedUnit) { // if no unit is selected
            if (unit) { 
                SelectUnit(unit); //select unit at clicked location
            }

            return; 
        }
        // if clicked tile is valid, and a unit is on that tile, and a unit is selected
        if (unit) {
            DeselectUnit();
            SelectUnit(unit);
            return;
        }
        
        //if (_selectedUnitMoves.Contains(gridPosition)) {
        //if clicked tile is valid, and a unit is selected, and another unit isnt on that tile, try move it to location
            _unitGridPositions.Remove(_selectedUnit.Position);
            _selectedUnit.Move(GetWorldPositionFromGrid(gridPosition), gridPosition);
            _unitGridPositions.Add(gridPosition, _selectedUnit);
            
            DeselectUnit();
        //}
    }

    [return: MaybeNull]
    public UnitComponent GetUnitAtGridPosition(Vector3Int gridPosition) {
        return _unitGridPositions.GetValueOrDefault(gridPosition);
    }

    private Vector3 GetWorldPositionFromGrid(Vector3Int gridPosition) {
        if (!_tileComponent.TryGetWorldPositionForTile(gridPosition, out var worldPos)) {
            Debug.Log($"Grid position {gridPosition} is invalid.");
            return Vector3.zero;
        }

        return worldPos;
    }
}