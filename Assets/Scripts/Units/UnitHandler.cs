using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.Tilemaps;
using TMPro;
using UnityEngine.UI;

public class UnitHandler : MonoBehaviour {
    [SerializeField]
    private Tilemap tileMap;

    [SerializeField]
    private DeployAreaComponent deployArea;

    [SerializeField]
    private GameObject unitInterface;

    [SerializeField]
    private GameObject unitCellPrefab;

    public UnitComponent pawn;
    public UnitComponent knight;
    public UnitComponent king;
    public UnitComponent barbarian;
    public UnitComponent jarl;

    public LongUnitComponent rook;
    public LongUnitComponent bishop;
    public LongUnitComponent queen;

    //eventually get dis from the lobby unit selection
    public List<UnitComponent> equippedUnits = new();

    private TileComponent _tileComponent;
    public  bool DeployMode {get; private set;}

    private readonly Dictionary<Vector3Int, UnitComponent> _unitGridPositions = new();

    private UnitComponent _selectedUnit;
    private readonly HashSet<Vector3Int> _selectedUnitMoves = new();

    private void Start() {
        _tileComponent = tileMap.GetComponent<TileComponent>();
        equippedUnits.Add(pawn);
        equippedUnits.Add(king);
        equippedUnits.Add(knight);

        PopulateUnitInterface();
    }

    public void DeployUnit(Vector3Int gridPos, UnitComponent unit) {
        if (deployArea.IsADeploy(gridPos)) {
            SpawnUnit(gridPos, unit);
        }
    }

    public void SpawnUnit(Vector3Int gridPos, UnitComponent unitType) {
        if (!_tileComponent.IsUnobstructedTile(gridPos)) {
            Debug.LogError($"Invalid grid position: {gridPos}");
            return;
        }

        var newUnit = Instantiate(unitType, Vector3.zero, Quaternion.identity, gameObject.transform);
        if (!_tileComponent.TryGetWorldPositionForTileCenter(gridPos, out var pos)) {
            Debug.LogError($"Failed to find tile center: {gridPos}");
            return;
        }

        newUnit.Move(pos,gridPos, true);
        _unitGridPositions.Add(gridPos, newUnit);
    }

    private void SelectUnit(UnitComponent unit) {
        if (_selectedUnit) {
            DeselectUnit();
        }

        _selectedUnit = unit;
        _selectedUnit.Select();

        var unitMoves = _selectedUnit.GetUnitMoves(_tileComponent, x => TryGetUnitAtGridPosition(x, out _));

        _selectedUnitMoves.AddRange(unitMoves.Select(x => x.pos));
        _tileComponent.SetTileHints(unitMoves.Select(x => x.pos));
    }

    private void DeselectUnit() {
        if (!_selectedUnit) {
            return;
        }

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

    private void PopulateUnitInterface() {
        foreach (UnitComponent unit in equippedUnits) {
            GameObject newCell = Instantiate(unitCellPrefab, unitInterface.transform);
            newCell.transform.Find("UnitName").GetComponent<TextMeshProUGUI>().text = unit.name;
            newCell.transform.Find("UnitTier").GetComponent<TextMeshProUGUI>().text = unit.currentTier.ToString();
            //newCell.transform.Find("UnitImage").GetComponent<Image>().sprite = unit.unitSprite;
            //add the sprite fields in later
            newCell.transform.Find("ActionPopout").gameObject.SetActive(false);
        }
    }
}