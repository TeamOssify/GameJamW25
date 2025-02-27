using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
    [SerializeField]
    private int[] levels;

    [SerializeField]
    private int selectedLevel;

    [SerializeField]
    private TextMeshProUGUI unitDetailsName;

    [SerializeField]
    private Image unitDetailsImage;

    public void RightArrow() {
        ChangeSelectedLevel(1);
    }

    public void LeftArrow() {
        ChangeSelectedLevel(-1);
    }

    private void ChangeSelectedLevel(int level) {
        selectedLevel += level;

        if (selectedLevel >= levels.Length) {
            selectedLevel = 0;
        }
        else if (selectedLevel < 0) {
            selectedLevel = levels.Length - 1;
        }
    }

    public void StartLevel() {
        if (selectedLevel >= levels.Length) {
            Debug.Log($"Tried to load level outside of bounds: {selectedLevel}");
            return;
        }

        SceneManager.LoadSceneAsync(levels[selectedLevel]);
    }

    public void SetUnitDetails(string unitName, Sprite sprite) {
        unitDetailsName.text = unitName;
        unitDetailsImage.sprite = sprite;
    }
}