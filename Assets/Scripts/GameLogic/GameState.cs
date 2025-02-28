using Eflatun.SceneReference;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour {
    [SerializeField]
    private TileComponent tileComponent;

    [SerializeField]
    private CapturePointHandler capturePointHandler;

    [SerializeField]
    private Canvas FailureScreen;

    [SerializeField]
    private SceneReference mainMenu;

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
        SceneManager.LoadScene(mainMenu.BuildIndex, LoadSceneMode.Single);
    }

    private void EnableInput() {
        tileComponent.Interactable = true;
        Debug.Log("Welcome back");
    }

    private void Update() {
        if (Input.GetKey(KeyCode.Escape)) {
            ReturnToMenu();
        }
    }
}
