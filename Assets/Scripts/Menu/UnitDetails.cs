using UnityEngine;
using UnityEngine.EventSystems;

public class UnitDetails : MonoBehaviour {
    [SerializeField]
    private string unitName;

    [SerializeField]
    private Sprite unitSprite;

    [SerializeField]
    private MainMenu mainMenu;

    public void OnPointerEnter(BaseEventData e) {
        Debug.Log("mouse over called");
        mainMenu.SetUnitDetails(unitName, unitSprite);
    }
}