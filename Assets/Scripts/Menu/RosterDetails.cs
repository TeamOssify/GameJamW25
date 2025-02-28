using UnityEngine;
using UnityEngine.UI;

public class RosterDetails : MonoBehaviour {
    [SerializeField]
    private Image image;

    [SerializeField]
    private UnitComponent unit;

    [SerializeField]
    private GameObject infoOverlay;

    [SerializeField]
    private MainMenu mainMenu;

    public void SetUnit(UnitComponent unit) {
        this.unit = unit;
        image.sprite = unit.UnitSprite;
        image.color = Color.white;
    }

    public void ClearUnit() {
        this.unit = null;
        image.sprite = null;
        image.color = Color.clear;
    }

    public void OnPointerEnter() {
        if (!unit) {
            return;
        }

        infoOverlay.SetActive(true);
        mainMenu.SetUnitDetails(unit);
    }

    public void OnPointerExit() {
        infoOverlay.SetActive(false);
    }

    public void OnClick() {
        mainMenu.DeselectUnit(unit);
    }
}