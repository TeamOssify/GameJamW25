using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    [SerializeField]
    private Dictionary<uint, GameObject> levels = new();

    [SerializeField]
    private uint currentLevel = 0;
}