using System.Collections.Generic;
using UnityEngine;

public class EnemyPortalComponent : MonoBehaviour {

    // Position of the portal
    public Vector3Int GridPos { get; private set; }

    // This feels like a bad solution, but its probably needed
    [SerializeField]
    private int portalId;

    public void Spawn(Vector3Int gridPosition) {
        GridPos = gridPosition;
    }
    void Start() { }

    void Update() { }
}