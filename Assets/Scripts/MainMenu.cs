using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private int[] levels = { 1 };
    private int selectedLevel = 0;

    public void rightArrow() {
        if(selectedLevel == levels.Length - 1) {
            selectedLevel = 0;
        }
        else {
            selectedLevel++;
        }
    }
    public void leftArrow() {
        if (selectedLevel == 0) {
            selectedLevel = levels.Length - 1;
        }
        else {
            selectedLevel--;
        }
    }

    public void startLevel() {
        SceneManager.LoadSceneAsync(levels[selectedLevel]);
    }
}
