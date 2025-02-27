using UnityEngine;

public class RosterSelector : MonoBehaviour {
    [SerializeField]
    private GameObject infoOverlay;

    public void OnPointerEnter() {
        infoOverlay.SetActive(true);
    }

    public void OnPointerExit() {
        infoOverlay.SetActive(false);
    }

    public void Start() {
        infoOverlay.SetActive(false);
    }
}