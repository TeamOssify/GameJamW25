using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class MovementComponentBase : MonoBehaviour {
    protected Tilemap MoveMap;

    protected void Awake() {
        if (!MoveMap && !TryGetComponent(out MoveMap)) {
            Debug.LogError($"Failed to find move set tilemap for {name}!");
        }
    }

    public abstract void GetMoves(MoveSet moveSet, Vector3Int unitPosition, TileComponent tileComponent);
}