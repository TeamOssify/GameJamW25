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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InfoOverlay.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
