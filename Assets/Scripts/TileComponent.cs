using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class TileComponent : MonoBehaviour {
    private const float TILE_Z = -1;

    [SerializeField]
    private GameObject tileHoverObject;

    [SerializeField]
    private UnitHandler unitHandler;
    
    private Transform _hoverTransform;
    private Vector3Int _hoverPosition;
    private SpriteRenderer _hoverRenderer;
    private Color _hoverColor;
    private bool _hoverDark;
    private bool _hoverVisible;

    [SerializeField]
    private GameObject tileHintObject;

    private readonly Dictionary<Vector3Int, GameObject> _tileHints = new();

    [SerializeField]
    private GameObject selectedTileObject;

    private readonly Dictionary<Vector3Int, GameObject> _selectedTiles = new();

    private Camera _mainCamera;
    private Tilemap _tileMap;

    private bool _isHoldingSelect;
    private Vector3Int _heldTile;

    internal void Start() {
        _hoverTransform = tileHoverObject.transform;
        _hoverRenderer = tileHoverObject.GetComponent<SpriteRenderer>();
        _hoverVisible = _hoverRenderer.enabled;

        _mainCamera = Camera.main;
        _tileMap = gameObject.GetComponent<Tilemap>();
    }

    internal void OnMouseOver() {
        UpdateHoveredTile();
    }

    private void UpdateHoveredTile() {
        // if (_isHoldingSelect && _hoverVisible) {
        //     _hoverRenderer.enabled = false;
        //     _hoverVisible = false;
        //     return;
        // }
        //
        // if (!_isHoldingSelect && !_hoverVisible) {
        //     _hoverRenderer.enabled = true;
        //     _hoverVisible = true;
        // }

        if (_isHoldingSelect) {
            if (!_hoverDark) {
                _hoverColor = _hoverRenderer.color;
                _hoverRenderer.color = Color.Lerp(_hoverColor, Color.clear, 0.33f);
                _hoverDark = true;
            }

            return;
        }

        if (_hoverDark) {
            _hoverRenderer.color = _hoverColor;
            _hoverDark = false;
        }

        var mousePos = GetMouseWorldPosition();
        if (TryGetTileForWorldPosition(mousePos, out var tilePos) && tilePos != _hoverPosition) {
            var tileCenter = _tileMap.GetCellCenterWorld(tilePos);
            _hoverTransform.position = new Vector3(tileCenter.x, tileCenter.y, TILE_Z);
            _hoverPosition = tilePos;
        }
    }

    internal void OnMouseDown() {
        if (!_isHoldingSelect) {
            _isHoldingSelect = true;
        }

        var mousePos = GetMouseWorldPosition();
        if (TryGetTileForWorldPosition(mousePos, out var tilePos)) {
            _heldTile = tilePos;
        }
    }

    internal void OnMouseUp() {
        var mousePos = GetMouseWorldPosition();
        if (TryGetTileForWorldPosition(mousePos, out var tilePos)) {
            if (tilePos == _heldTile) {
                SelectTile(tilePos);
                unitHandler.SelectTile(tilePos);
            }
        }

        if (_isHoldingSelect) {
            _isHoldingSelect = false;
            _heldTile = Vector3Int.zero;
        }
    }

    private Vector3 GetMouseWorldPosition() {
        return _mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
    }

    private void SelectTile(Vector3Int tilePos) {
        var cellCenter = _tileMap.GetCellCenterWorld(tilePos);
        if (_selectedTiles.Remove(tilePos, out var oldObject)) {
            Destroy(oldObject);
        }
        else {
            var spawnPos = new Vector3(cellCenter.x, cellCenter.y, TILE_Z);
            _selectedTiles[tilePos] = Instantiate(selectedTileObject, spawnPos, Quaternion.identity);
        }
    }

    public bool TryGetWorldPositionForTileCenter(Vector3Int tilePos, out Vector3 worldPos) {
        if (!IsValidTile(tilePos)) {
            // Debug.Log($"Clicked out of bounds of tile map! Tried to fetch cell at {cellPos}");
            worldPos = Vector3Int.zero;
            return false;
        }

        worldPos = _tileMap.GetCellCenterWorld(tilePos);
        return true;
    }

    public bool TryGetWorldPositionForTile(Vector3Int tilePos, out Vector3 worldPos) {
        if (!IsValidTile(tilePos)) {
            // Debug.Log($"Clicked out of bounds of tile map! Tried to fetch cell at {cellPos}");
            worldPos = Vector3Int.zero;
            return false;
        }

        worldPos = _tileMap.CellToWorld(tilePos);
        return true;
    }

    public bool TryGetTileForWorldPosition(Vector3 worldPos, out Vector3Int tilePos) {
        var cellPos = _tileMap.WorldToCell(worldPos);

        if (!IsValidTile(cellPos)) {
            // Debug.Log($"Clicked out of bounds of tile map! Tried to fetch cell at {cellPos}");
            tilePos = Vector3Int.zero;
            return false;
        }

        tilePos = cellPos;
        return true;
    }

    public bool IsValidTile(Vector3Int pos) {
        // TODO: Collision check against valid tile mask?
        return _tileMap.HasTile(pos);
    }

    public void DebugSetTileHints() {
        var hints = new Vector3Int[] {
            new(-1, 0, 0),
            new(1, 0, 0),
            new(0, -1, 0),
            new(0, 1, 0),
            new(-3, -3, 0),
            new(-3, 0, 0),
        };

        SetTileHints(hints);
    }

    public void SetTileHints(Vector3Int[] hints) {
        ClearTileHints();

        foreach (var hintPos in hints) {
            if (!_tileHints.ContainsKey(hintPos)) {
                var cellCenter = _tileMap.GetCellCenterWorld(hintPos);
                var spawnPos = new Vector3(cellCenter.x, cellCenter.y, TILE_Z);
                _tileHints[hintPos] = Instantiate(tileHintObject, spawnPos, Quaternion.identity);
            }
        }
    }

    public void ForeachHint(Action<Vector3Int, GameObject> predicate) {
        foreach (var (pos, hint) in _tileHints) {
            predicate(pos, hint);
        }
    }

    public void ClearTileHints() {
        foreach (var oldHint in _tileHints.Values) {
            Destroy(oldHint);
        }

        _tileHints.Clear();
    }
}