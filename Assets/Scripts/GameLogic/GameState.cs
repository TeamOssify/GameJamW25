using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour {
    [SerializeField]
    private TileComponent tileComponent;

    [SerializeField]
    private CapturePointHandler capturePointHandler;

    [SerializeField]
    private Canvas FailureScreen;

    private void Awake() {
        capturePointHandler.allPointsCaptured.AddListener(DisableInput);
    }

    private void OnDestroy() {
        capturePointHandler.allPointsCaptured.RemoveListener(DisableInput);
    }

    private void DisableInput() {
        tileComponent.Interactable = false;
        FailureScreen.gameObject.SetActive(true);
        Debug.Log("Game over");
    }

    public void ReturnToMenu() {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    private void EnableInput() {
        tileComponent.Interactable = true;
        Debug.Log("Welcome back");
    }
}
