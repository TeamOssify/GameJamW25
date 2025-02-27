using UnityEngine;

public class RosterSelector : MonoBehaviour
{
    [SerializeField]
    private GameObject InfoOverlay;

    public void OnPointerEnter() {
        InfoOverlay.SetActive(true);
    }

    public void OnPointerExit() {
        InfoOverlay.SetActive(false);
    }
    

}
