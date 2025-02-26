using System;
using UnityEngine;

[Serializable]
public class Wave : MonoBehaviour {
    public uint WaveNumber => waveNumber;

    [SerializeField]
    private uint waveNumber;

    public SubWave[] SubWaves => subWaves;

    [SerializeField]
    private SubWave[] subWaves;
}