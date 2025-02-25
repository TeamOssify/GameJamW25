using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class TileComponent : MonoBehaviour {
    private const float TILE_Z = -1;

    [SerializeField]
    private GameObject tileHoverObject;

    private Transform _hoverTransform;
    private Vector3Int _hoverPosition;
    private Renderer _hoverRenderer;
    private bool _hoverVisible;

    [SerializeField]
    private GameObject selectedTileObject;

    private readonly Dictionary<Vector3Int, GameObject> _selectedTiles = new();

    private Camera _mainCamera;
    private Tilemap _tileMap;

    private bool _isHoldingSelect;
    private Vector3Int _heldTile;

    internal void Start() {
        _hoverTransform = tileHoverObject.transform;
        _hoverRenderer = tileHoverObject.GetComponent<Renderer>();
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
            return;
        }

        var mousePos = GetMouseWorldPosition();
        if (TryGetTileForWorldPosition(mousePos, out var tilePos) && tilePos != _hoverPosition) {
            var tileCenter = _tileMap.GetCellCenterWorld(tilePos);
            _hoverTransform.position = new Vector3(tileCenter.x, tileCenter.y, TILE_Z);
            _hoverPosition = tilePos;
        }
    }

    internal void OnMouseDown() {
        _isHoldingSelect = true;

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
            }
        }

        _isHoldingSelect = false;
        _heldTile = Vector3Int.zero;
    }


    private Vector3 GetMouseWorldPosition() {
        return _mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
    }

    private void SelectTile(Vector3Int tilePos) {
        var cellCenterWorld = _tileMap.GetCellCenterWorld(tilePos);
        if (_selectedTiles.Remove(tilePos, out var oldObject)) {
            Destroy(oldObject);
        }
        else {
            var spawnPos = new Vector3(cellCenterWorld.x, cellCenterWorld.y, TILE_Z);
            _selectedTiles[tilePos] = Instantiate(selectedTileObject, spawnPos, Quaternion.identity);
        }
    }

    private bool TryGetTileForWorldPosition(Vector3 worldPos, out Vector3Int tilePos) {
        var cellPos = _tileMap.WorldToCell(worldPos);

        if (!_tileMap.HasTile(cellPos)) {
            Debug.Log($"Clicked out of bounds of tile map! Tried to fetch cell at {cellPos}");
            tilePos = Vector3Int.zero;
            return false;
        }

        tilePos = cellPos;
        return true;
    }
}