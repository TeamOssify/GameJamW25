using System;
using UnityEngine;

[Serializable]
public class SubWave : MonoBehaviour {
    public uint SpawnIndex => spawnIndex;

    [SerializeField]
    [Tooltip("Used to control when a subwave spawn occurs. Ex: 2 will not spawn until 1 is clear.")]
    private uint spawnIndex;

    public GameObject EnemyToSpawn => enemyToSpawn;

    [SerializeField]
    private GameObject enemyToSpawn;

    public uint EnemyCount => enemyCount;

    [SerializeField]
    private uint enemyCount;

    [Tooltip("Forces all enemies to be spawned at the same portal instead of randomly.")]
    public bool ForceSpawnTogether => forceSpawnTogether;

    [SerializeField]
    private bool forceSpawnTogether;
}