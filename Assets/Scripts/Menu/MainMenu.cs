using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
    [SerializeField]
    private GameObject levels;

    private Level[] _levelList;

    [SerializeField]
    private int selectedLevel;

    [SerializeField]
    private TextMeshProUGUI levelText;

    [SerializeField]
    private TextMeshProUGUI unitDetailsName;

    [SerializeField]
    private TextMeshProUGUI unitDetailsDescription;

    [SerializeField]
    private Image unitFirstMoveImage;

    [SerializeField]
    private Image unitNormalMoveImage;

    [SerializeField]
    private RosterDetails[] rosterSlots;

    [SerializeField]
    private UnitMovementRenderer movementRenderer;

    private readonly HashSet<UnitComponent> _selectedUnits = new();

    [SerializeField]
    private UnitRosterManagerScriptableObject unitRosterManager;

    private void Awake() {
        _levelList = levels.GetComponents<Level>().OrderBy(x => x.levelId).ToArray();
    }

    private void Start() {
        ChangeSelectedLevel(0);
    }

    public void RightArrow() {
        ChangeSelectedLevel(1);
    }

    public void LeftArrow() {
        ChangeSelectedLevel(-1);
    }

    private void ChangeSelectedLevel(int level) {
        selectedLevel += level;

        if (selectedLevel >= _levelList.Length) {
            selectedLevel = 0;
        }
        else if (selectedLevel < 0) {
            selectedLevel = _levelList.Length - 1;
        }

        levelText.text = _levelList[selectedLevel].LevelName;
    }

    public void StartLevel() {
        if (selectedLevel >= _levelList.Length) {
            Debug.Log($"Tried to load level outside of bounds: {selectedLevel}");
            return;
        }

        if (_selectedUnits.Count < 1) {
            return;
        }

        unitRosterManager.UnitRoster = _selectedUnits;

        SceneManager.LoadSceneAsync(_levelList[selectedLevel].LevelScene.BuildIndex);
    }

    public void SetUnitDetails(UnitComponent unit) {
        unitDetailsName.text = unit.UnitName;
        unitDetailsDescription.text = unit.UnitDescription;
        // unitDetailsImage.sprite = unit.UnitSprite;
        var moves = movementRenderer.RenderUnitMovement(unit);
        unitFirstMoveImage.sprite = moves.FirstMove;
        unitNormalMoveImage.sprite = moves.NormalMove;
    }

    public void SelectUnit(UnitComponent unit) {
        if (_selectedUnits.Count >= rosterSlots.Length) {
            if (_selectedUnits.Contains(unit)) {
                DeselectUnit(unit);
            }

            return;
        }

        if (!_selectedUnits.Add(unit)) {
            DeselectUnit(unit);
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