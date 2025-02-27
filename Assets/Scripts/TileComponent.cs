using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class TileComponent : MonoBehaviour {
    private const float TILE_Z = -1;

    [SerializeField]
    private GameObject tileHoverObject;

    [SerializeField]
    private MovementMaskComponent movementMask;

    private Transform _hoverTransform;
    private Vector3Int _hoverPosition;
    private SpriteRenderer _hoverRenderer;
    private Color _hoverColor;
    private bool _hoverDark;
    private bool _hoverVisible;

    [FormerlySerializedAs("tileHintObject")]
    [SerializeField]
    private GameObject debugHintObject;

    private readonly Dictionary<HintBucket, Dictionary<Vector3Int, GameObject>> _tileHints = new();

    private Camera _mainCamera;
    private Tilemap _tileMap;

    private bool _isHoldingSelect;
    private Vector3Int _heldTile;

    public EventHandler<Vector3Int> OnTileSelected;

    internal void Start() {
        _hoverTransform = tileHoverObject.transform;
        _hoverRenderer = tileHoverObject.GetComponent<SpriteRenderer>();
        _hoverVisible = _hoverRenderer.enabled;

        _mainCamera = Camera.main;
        _tileMap = gameObject.GetComponent<Tilemap>();

        foreach (HintBucket value in Enum.GetValues(typeof(HintBucket))) {
            if (value is HintBucket.All) {
                continue;
            }

            _tileHints[value] = new Dictionary<Vector3Int, GameObject>();
        }
    }

    internal void OnMouseOver() {
        UpdateHoveredTile();
    }

    private void UpdateHoveredTile() {
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
        if (!IsValidTile(tilePos)) {
            return;
        }

        OnTileSelected?.Invoke(this, tilePos);
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

    public virtual bool IsValidTile(Vector3Int pos) {
        return _tileMap.HasTile(pos);
    }

    public virtual bool IsUnobstructedTile(Vector3Int pos) {
        return IsValidTile(pos) && !movementMask.IsPositionBlocked(pos);
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

        SetTileHints(HintBucket.NormalMove, hints, debugHintObject);
    }

    public void SetTileHints(HintBucket hintBucket, IEnumerable<Vector3Int> hints, GameObject hintObject) {
        ClearTileHints(hintBucket);
        AddTileHints(hintBucket, hints, hintObject);
    }

    public void AddTileHints(HintBucket hintBucket, IEnumerable<Vector3Int> hints, GameObject hintObject) {
        if (hintBucket is HintBucket.All) {
            Debug.LogError("Cannot add hints to all buckets!");
            return;
        }

        var bucket = _tileHints[hintBucket];
        foreach (var hintPos in hints) {
            if (bucket.ContainsKey(hintPos)) {
                continue;
            }

            var cellCenter = _tileMap.GetCellCenterWorld(hintPos);
            var spawnPos = new Vector3(cellCenter.x, cellCenter.y, TILE_Z);
            var hintObj = Instantiate(hintObject, spawnPos, Quaternion.identity);
            hintObj.GetComponent<SpriteRenderer>().enabled = true;
            bucket[hintPos] = hintObj;
        }
    }

    public void ForeachHint(HintBucket hintBucket, Action<Vector3Int, GameObject> predicate) {
        if (hintBucket is HintBucket.All) {
            foreach (var bucket in _tileHints.Keys.ToArray()) {
                ForeachHint(bucket, predicate);
            }

            return;
        }

        foreach (var (pos, hint) in _tileHints[hintBucket]) {
            predicate(pos, hint);
        }
    }

    public void ClearAllTileHints() {
        ClearTileHints(HintBucket.All);
    }

    public void ClearTileHints(HintBucket hintBucket) {
        if (hintBucket is HintBucket.All) {
            foreach (var bucket in _tileHints.Keys.ToArray()) {
                ClearTileHints(bucket);
            }

            return;
        }

        foreach (var oldHint in _tileHints[hintBucket].Values) {
            Destroy(oldHint);
        }

        _tileHints[hintBucket].Clear();
    }
}