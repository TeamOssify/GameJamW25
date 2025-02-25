using Sherbert.Framework.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    [SerializeField]
    private SerializableDictionary<uint, GameObject> levels;

    [SerializeField]
    private uint currentLevel;
}