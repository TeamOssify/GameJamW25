using UnityEngine;
using UnityEngine.Tilemaps;

public class MovementMaskComponent : MonoBehaviour {
    private Tilemap _tilemap;

    private void Start() {
        _tilemap = GetComponent<Tilemap>();

#if !UNITY_EDITOR
        // Ensure mask is hidden at runtime
        var tilemapRenderer = GetComponent<TilemapRenderer>();
        if (tilemapRenderer && tilemapRenderer.enabled) {
            tilemapRenderer.enabled = false;
        }
#endif
    }

    public bool IsPositionBlocked(Vector3Int gridPosition) {
        return _tilemap.HasTile(gridPosition);
    }
}