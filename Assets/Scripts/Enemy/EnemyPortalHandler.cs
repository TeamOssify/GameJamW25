using System.Collections.Generic;
using UnityEngine;

public class EnemyPortalHandler : MonoBehaviour {

    // Im gonna be so fr I have no idea how dictionaries work owo
    // Update I think I know how they work
    private Dictionary<Vector3Int, int> _enemyPortalPositions = new();

    public void CreatePortal(Vector3Int initPos, EnemyPortalComponent portal) {

        var newPortal = Instantiate(portal, initPos, Quaternion.identity, gameObject.transform);
        newPortal.Spawn(initPos);
        _enemyPortalPositions.Add(initPos, _enemyPortalPositions.Count);
    }
    public void SpawnSubwave(SubWave wave, EnemyPortalComponent portal) {
        
    }
}