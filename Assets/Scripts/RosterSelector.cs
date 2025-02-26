using UnityEngine;

public class RosterSelector : MonoBehaviour
{
    public GameObject InfoOverlay;

    public void openOverlay() {
        InfoOverlay.SetActive(true);
    }

    public void closeOverlay() {
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
