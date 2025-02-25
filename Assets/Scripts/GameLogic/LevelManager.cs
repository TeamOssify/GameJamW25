using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    [SerializeField]
    private List<Level> levels;

    [SerializeField]
    private uint currentLevel;

    public void ChangeLevel(uint newLevel) { }
}