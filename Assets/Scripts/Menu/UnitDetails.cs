using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnitDetails : MonoBehaviour {
    private UnitComponent _unit;
    private MainMenu _mainMenu;

    [SerializeField]
    private TextMeshProUGUI unitNameText;

    [SerializeField]
    private Image unitImage;

    private bool _canInit = true;

    public void Init(UnitComponent unit, MainMenu mainMenu) {
        if (!_canInit) {
            Debug.LogWarning("Tried to init an already initialized unit detail!");
            return;
        }

        _canInit = false;

        _unit = unit;
        _mainMenu = mainMenu;

        unitNameText.text = _unit.UnitName;
        unitImage.sprite = _unit.UnitSprite;
    }

    public void OnPointerEnter(BaseEventData e) {
        _mainMenu.SetUnitDetails(_unit);
    }

    public void ButtonPress() {
        _mainMenu.SelectUnit(_unit);
    }
}