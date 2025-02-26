using UnityEngine;

public class UnitDetails : MonoBehaviour {
    [SerializeField]
    private string unitName;

    [SerializeField]
    private Sprite unitSprite;

    [SerializeField]
    private MainMenu mainMenu;

    private void OnMouseEnter() {
        mainMenu.SetUnitDetails(unitName, unitSprite);
    }
}