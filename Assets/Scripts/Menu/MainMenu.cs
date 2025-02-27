using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
    [SerializeField]
    private Level[] levels;

    [SerializeField]
    private int selectedLevel;

    [SerializeField]
    private TextMeshProUGUI unitDetailsName;

    [SerializeField]
    private TextMeshProUGUI unitDetailsDescription;

    [SerializeField]
    private Image unitDetailsImage;

    [SerializeField]
    private RosterDetails[] rosterSlots;

    [SerializeField]
    private UnitMovementRenderer movementRenderer;

    private readonly HashSet<UnitComponent> _selectedUnits = new();

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

        SceneManager.LoadSceneAsync(levels[selectedLevel].LevelScene.BuildIndex);
    }

    public void SetUnitDetails(UnitComponent unit) {
        unitDetailsName.text = unit.UnitName;
        unitDetailsDescription.text = unit.UnitDescription;
        // unitDetailsImage.sprite = unit.UnitSprite;
        unitDetailsImage.sprite = movementRenderer.RenderUnitMovement(unit);
    }

    public void SelectUnit(UnitComponent unit) {
        if (_selectedUnits.Count >= rosterSlots.Length) {
            return;
        }

        if (!_selectedUnits.Add(unit)) {
            return;
        }

        UpdateRoster();
    }

    public void DeselectUnit(UnitComponent unit) {
        if (!_selectedUnits.Remove(unit)) {
            return;
        }

        UpdateRoster();
    }

    private void UpdateRoster() {
        var i = 0;
        foreach (var unit in _selectedUnits) {
            rosterSlots[i].SetUnit(unit);
            i++;
        }

        for (; i < rosterSlots.Length; i++) {
            rosterSlots[i].ClearUnit();
        }
    }
}