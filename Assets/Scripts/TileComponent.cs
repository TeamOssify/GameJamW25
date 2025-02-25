using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class TileComponent : MonoBehaviour {
    [SerializeField]
    private GameObject selectedTileObject;
    private readonly Dictionary<Vector3Int, GameObject> _selectedTiles = new();

    private Camera _mainCamera;
    private Tilemap _tileMap;

    internal void Start() {
        _mainCamera = Camera.main;
        _tileMap = gameObject.GetComponent<Tilemap>();
    }

    internal void OnMouseUpAsButton() {
        var mousePos = _mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        var cellPos = _tileMap.WorldToCell(mousePos);
        if (!_tileMap.HasTile(cellPos)) {
            Debug.Log($"Clicked out of bounds of tile map! Tried to fetch cell at {cellPos}");
            return;
        }

        var cellCenterWorld = _tileMap.GetCellCenterWorld(cellPos);
        if (_selectedTiles.Remove(cellPos, out var oldObject)) {
            Destroy(oldObject);
        }
        else {
            var spawnPos = cellCenterWorld + new Vector3(0, 0, -0.1f);
            _selectedTiles[cellPos] = Instantiate(selectedTileObject, spawnPos, Quaternion.identity);
        }
    }
}
