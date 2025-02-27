using UnityEngine;
using UnityEngine.Events;

public class CapturePoint : MonoBehaviour
{
    public UnityEvent captured;
    public Vector3Int GridPosition {get; private set;}

    public void Move(Vector3 worldPos, Vector3Int gridPos) {
        transform.position = worldPos;
        GridPosition = gridPos;
    }
    
    // Should be linked to UnitHandler's unitMoved
    public void OnUnitMove(object sender, Vector3Int pos) {
        if (pos == GridPosition) {
            captured.Invoke();
            Destroy(gameObject);
        }
    }
}
