using System;
using Eflatun.SceneReference;
using UnityEngine;

[Serializable]
public class Level : MonoBehaviour {
    public int levelId;

    public string LevelName => levelName;

    [SerializeField]
    private string levelName;

    public SceneReference LevelScene => levelScene;

    [SerializeField]
    private SceneReference levelScene;
}