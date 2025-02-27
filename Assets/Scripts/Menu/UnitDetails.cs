using UnityEngine;
using UnityEngine.EventSystems;

public class UnitDetails : MonoBehaviour {
    [SerializeField]
    private UnitComponent unit;

    [SerializeField]
    private MainMenu mainMenu;

    public void OnPointerEnter(BaseEventData e) {
        mainMenu.SetUnitDetails(unit);
    }

    public void ButtonPress() {
        mainMenu.SelectUnit(unit);
    }
}