using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    private int[] levels = { 1 };
    private int selectedLevel = 0;

    public void RightArrow() {
        selectedLevel++;
        if (selectedLevel >= levels.Length) {
            selectedLevel = 0;
        }
    }

    public void LeftArrow() {
        selectedLevel--;
        if (selectedLevel < 0) {
            selectedLevel = levels.Length - 1;
        }
    }

    public void StartLevel() {
        SceneManager.LoadSceneAsync(levels[selectedLevel]);
    }
}