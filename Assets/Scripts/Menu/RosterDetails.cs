using UnityEngine;
using UnityEngine.UI;
public class RosterDetails : MonoBehaviour
{
    [SerializeField]
    private Image image;

    [SerializeField]
    private UnitComponent unit;

    [SerializeField]
    private GameObject InfoOverlay;

    [SerializeField]
    private MainMenu mainMenu;


    public void SetUnit(UnitComponent unit) {
        this.unit = unit;
        Debug.Log(image);
        image.sprite = unit.UnitSprite;
 
    }

    public void ClearUnit() {
        this.unit = null;
        image.sprite = null;
    }

    public void OnPointerEnter() {
        if(unit == null) {
            return;
        }
        InfoOverlay.SetActive(true);
        mainMenu.SetUnitDetails(unit.UnitName, unit.UnitDescription, unit.UnitSprite);
    }

    public void OnPointerExit() {
        InfoOverlay.SetActive(false);
    }

    public void OnClick() {
        mainMenu.DeselectUnit(unit);
    }
}
