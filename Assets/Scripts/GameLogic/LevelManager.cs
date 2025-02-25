using UnityEngine;

public class LevelManager : MonoBehaviour {
    [SerializeField]
    private Level[] levels;

    [SerializeField]
    private uint currentLevel;

    public void ChangeLevel(uint newLevel) { }
}