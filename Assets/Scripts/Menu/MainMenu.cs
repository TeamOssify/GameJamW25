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
    private TextMeshPro unitDetailsName;

    [SerializeField]
    private Image unitDetailsImage;

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

    public void SetUnitDetails(string unitName, Sprite sprite) {
        unitDetailsName.text = unitName;
        unitDetailsImage.sprite = sprite;
    }
}