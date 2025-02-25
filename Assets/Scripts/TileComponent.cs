using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class TileComponent : MonoBehaviour {
    private Camera _mainCamera;
    private Tilemap _tileMap;

    internal void Start() {
        _mainCamera = Camera.main;
        _tileMap = gameObject.GetComponent<Tilemap>();
    }

    internal void OnMouseUpAsButton() {
        var mousePos = Mouse.current.position;
        var mouseVec = new Vector3(mousePos.x.value, mousePos.y.value);
        var worldPos = _mainCamera.ScreenToWorldPoint(mouseVec);

        var cellPos = _tileMap.WorldToCell(worldPos);
        var hasTile = _tileMap.HasTile(cellPos);
        Debug.Log($"{worldPos} - {cellPos} {hasTile}");
    }
}
