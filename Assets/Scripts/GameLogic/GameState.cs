using UnityEngine;

public class GameState : MonoBehaviour {
    [SerializeField]
    private TileComponent tileComponent;

    [SerializeField]
    private CapturePointHandler capturePointHandler;

    private void Awake() {
        capturePointHandler.allPointsCaptured.AddListener(DisableInput);
    }

    private void OnDestroy() {
        capturePointHandler.allPointsCaptured.RemoveListener(DisableInput);
    }

    private void DisableInput() {
        tileComponent.Interactable = false;
    }

    private void EnableInput() {
        tileComponent.Interactable = true;
    }
}
