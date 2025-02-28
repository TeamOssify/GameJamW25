using System;
using UnityEngine;

[Serializable]
public class SubWave : MonoBehaviour {
    public EnemyComponent EnemyToSpawn => enemyToSpawn;

    [SerializeField]
    private EnemyComponent enemyToSpawn;

    public uint EnemyCount => enemyCount;

    [SerializeField]
    private uint enemyCount;
}