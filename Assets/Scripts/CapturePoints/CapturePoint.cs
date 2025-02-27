using UnityEngine;
using UnityEngine.Events;

public class CapturePoint : MonoBehaviour
{
    public UnityEvent captured;
    public Vector3Int gridPosition {get; private set;}

    public void Move(Vector3 worldPos, Vector3Int gridPos) {
        transform.position = worldPos;
        gridPosition = gridPos;
    }
    
    // Should be linked to UnitHandler's unitMoved
    public void onUnitMove(Vector3Int pos) {
        if (pos == gridPosition) {
            captured.Invoke();
            Destroy(gameObject);
        }
    }
}
