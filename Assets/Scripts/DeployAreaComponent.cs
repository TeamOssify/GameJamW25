using UnityEngine;
using UnityEngine.Tilemaps;

public class DeployAreaComponent : MonoBehaviour {
    private Tilemap _tilemap;
    private TilemapRenderer _tilemapRenderer;
    private void Start() {
        _tilemap = GetComponent<Tilemap>();
        _tilemapRenderer = GetComponent<TilemapRenderer>();
        ToggleDeployAreaVisibility();
    }

    public bool IsADeploy(Vector3Int gridPosition) {
        return _tilemap.HasTile(gridPosition);
    }

    public void ToggleDeployAreaVisibility() {
        Debug.Log("Deploy area visible");
        if (_tilemapRenderer) {
            _tilemapRenderer.enabled = !_tilemapRenderer.enabled;
        }
    }
}

