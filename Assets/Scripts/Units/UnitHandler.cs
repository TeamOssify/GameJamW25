using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.Tilemaps;
using TMPro;
using UnityEngine.UI;
using System;
using System.Linq;

public class UnitHandler : MonoBehaviour {
    [SerializeField]
    private Tilemap tileMap;

    [SerializeField]
    private DeployAreaComponent deployArea;

    [SerializeField]
    private GameObject unitInterface;

    [SerializeField]
    private GameObject unitCellPrefab;

    [SerializeField]
    private CapturePointHandler capturePointHandler;

    [SerializeField]
    private TurnStateManager turnStateManager;

    [SerializeField]
    private EnemyHandler enemyHandler;

    public UnitComponent pawn;
    public UnitComponent knight;
    public UnitComponent king;
    public UnitComponent barbarian;
    public UnitComponent jarl;
    public UnitComponent rook;
    public UnitComponent bishop;
    public UnitComponent queen;

    //eventually get dis from the lobby unit selection
    public List<UnitComponent> equippedUnits = new();

    private TileComponent _tileComponent;
    public bool DeployMode;
    private UnitComponent _deployingUnit;

    public IEnumerable<Vector3Int> UnitPositions => _unitGridPositions.Keys;

    private readonly Dictionary<Vector3Int, UnitComponent> _unitGridPositions = new();

    private UnitComponent _selectedUnit;
    private readonly HashSet<Vector3Int> _selectedUnitMoves = new();
    private readonly HashSet<UnitComponent> _movedUnits = new();

    public EventHandler<Vector3Int> UnitMoved;

    [SerializeField]
    private GameObject normalMoveHint;

    [SerializeField]
    private GameObject jumpMoveHint;

    [SerializeField]
    private GameObject captureMoveHint;

    [SerializeField]
    private AudioSource click;

    [SerializeField]
    private AudioSource move;

    private void Start() {
        _tileComponent = tileMap.GetComponent<TileComponent>();
        _tileComponent.OnTileSelected += SelectTile;

        turnStateManager.OnNextPlayerTurn += OnNextPlayerTurn;
    }

    private void OnNextPlayerTurn(object sender, EventArgs e) {
        _movedUnits.Clear();
    }

    private void OnDestroy() {
        // I would prefer if these were in OnEnable and OnDisable but we don't have a choice
        _tileComponent.OnTileSelected -= SelectTile;
    }

    public void InitDeploy() {
        //eventually, we'll grab the stuff from main menu here
        equippedUnits.Add(pawn);
        equippedUnits.Add(king);
        equippedUnits.Add(knight);
        equippedUnits.Add(queen);

        PopulateUnitInterface();
        turnStateManager.EnterDeployment();
    }
    
    public void SetUnitForDeployment(UnitComponent unit) {
        _deployingUnit = unit;
    }

    public void DeployUnit(Vector3Int gridPos, UnitComponent unit) {
        if (deployArea.IsADeploy(gridPos)) {
            SpawnUnit(gridPos, unit);
            Debug.Log("Spawned unit");
            if (turnStateManager.CurrentTurnState == TurnStateManager.TurnState.Deployment) {
                turnStateManager.UnitDeployed();
            }
            else {
                // its a respawn
            }
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
        if (ReferenceEquals(_selectedUnit, unit)) {
            // If the user clicked the same unit more than once, they probably want to deselect
            DeselectUnit();
            return;
        }

        if (_selectedUnit) {
            DeselectUnit();
        }

        if (_movedUnits.Contains(unit)) {
            return;
        }

        _selectedUnit = unit;
        _selectedUnit.Select();

        // ReSharper disable once ConvertToLambdaExpression
        Predicate<Vector3Int> moveFilter = x => {
            return TryGetUnitAtGridPosition(x, out _) || capturePointHandler.IsCapturePoint(x);
        };

        var unitMoves = _selectedUnit.GetUnitMoves(_tileComponent, moveFilter, moveFilter);

        var captureMoves = new List<Vector3Int>();
        captureMoves.AddRange(unitMoves.NormalMoves.Where(x => enemyHandler.WouldCaptureEnemy(x)));
        captureMoves.AddRange(unitMoves.JumpMoves.Where(x => enemyHandler.WouldCaptureEnemy(x)));

        _selectedUnitMoves.AddRange(unitMoves.NormalMoves);
        _selectedUnitMoves.AddRange(unitMoves.JumpMoves);

        _tileComponent.SetTileHints(HintBucket.NormalMove, unitMoves.NormalMoves, normalMoveHint);
        _tileComponent.SetTileHints(HintBucket.JumpMove, unitMoves.JumpMoves, jumpMoveHint);
        _tileComponent.SetTileHints(HintBucket.CaptureMove, captureMoves, captureMoveHint);
    }

    private void DeselectUnit() {
        if (!_selectedUnit) {
            return;
        }

        _selectedUnit.Deselect();
        _selectedUnitMoves.Clear();
        _tileComponent.ClearAllTileHints();
        _selectedUnit = null;
    }


    private void SelectTile(object sender, Vector3Int gridPosition) {

        click.Play();   // play click sfx

        if (!_tileComponent.IsUnobstructedTile(gridPosition)) {
            // Blocked tile
            DeselectUnit();
            return;
        }

        if (TryGetUnitAtGridPosition(gridPosition, out var unit)) {
            // Clicked a unit
            if (!DeployMode) {
                SelectUnit(unit);
            }

            return;
        }

        if (DeployMode && _deployingUnit != null) {
            Debug.Log("deployed unit");
            DeployUnit(gridPosition, _deployingUnit);
            return;
        }

        if (!_selectedUnit) {
            // No unit selected
            return;
        }

        if (enemyHandler.WouldCaptureEnemy(gridPosition)) {
            if(enemyHandler.TryGetEnemyAtGridPosition(gridPosition, out var enemy)){
                enemyHandler.CaptureEnemy(gridPosition, enemy);
            }
        }

        if (_selectedUnitMoves.Contains(gridPosition)
            && _tileComponent.TryGetWorldPositionForTileCenter(gridPosition, out var worldPos)
            && _movedUnits.Add(_selectedUnit)
           ) {
            // Unit selected, clicked valid move tile
            _unitGridPositions.Remove(_selectedUnit.GridPos);
            _selectedUnit.Move(worldPos, gridPosition);
            _unitGridPositions.Add(gridPosition, _selectedUnit);

            UnitMoved?.Invoke(this, gridPosition);
            click.Stop();
            move.Play();
        }

        DeselectUnit();
    }

    public bool TryGetUnitAtGridPosition(Vector3Int gridPosition, out UnitComponent unit) {
        return _unitGridPositions.TryGetValue(gridPosition, out unit);
    }

    private void PopulateUnitInterface() {
        foreach (var unit in equippedUnits) {
            var newCell = Instantiate(unitCellPrefab, unitInterface.transform);
            newCell.transform.Find("UnitName").GetComponent<TextMeshProUGUI>().text = unit.name;
            newCell.transform.Find("UnitTier").GetComponent<TextMeshProUGUI>().text = unit.currentTier.ToString();
            newCell.transform.Find("UnitImage").GetComponent<Image>().sprite = unit.unitSprite;
            //add the sprite fields in later
            GameObject actionPopout = newCell.transform.Find("ActionPopout").gameObject;
            actionPopout.SetActive(false);

            Button openButton = newCell.transform.Find("OpenActionPopout").GetComponent<Button>();

            openButton.onClick.AddListener(() => {
                Debug.Log("added listener to buttone");
                actionPopout.SetActive(!actionPopout.activeSelf);
            });
        }
    }
}