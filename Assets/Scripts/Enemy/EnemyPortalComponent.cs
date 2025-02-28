using UnityEngine;

public class EnemyPortalComponent : MonoBehaviour {

    // Position of the portal
    public Vector3Int GridPos { get; private set; }

    // This feels like a bad solution, but its probably needed
    [SerializeField]
    private int portalId;

    public bool spawnedAllUnits;

    public int currentWave;
    public void SpawnPortal(Vector3Int gridPosition) {
        GridPos = gridPosition;
    }

    public void IncreaseWaveCount() {
        currentWave++;
    }
}