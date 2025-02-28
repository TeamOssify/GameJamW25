using System.Collections.Generic;
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
    
    // Should be linked to EnemyHandler's enemiesMoveds
    public void OnEnemyMove(object sender, Dictionary<Vector3Int, EnemyComponent> enemyPositions) {
        if (enemyPositions.ContainsKey(GridPosition)) {
            captured.Invoke();
            Destroy(gameObject);
        }
    }
}
