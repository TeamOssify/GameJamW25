using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class MainHudController : MonoBehaviour
{
    private int currentTurn = 1;
    
    // UI Element(s)
    private Label hudTurnCounter;

    public void UpdateTurnCount(int count) {
        currentTurn = count;
        hudTurnCounter.text = currentTurn.ToString();
    }

    public void Start() {
        // Load UI elements
        hudTurnCounter = GetComponent<UIDocument>().rootVisualElement.Q<Label>("TurnNumber");

        UpdateTurnCount(1);
    }
    public void Update() {
        if (Input.GetKeyDown(KeyCode.RightBracket)) {
            UpdateTurnCount(currentTurn + 1); // For testing, remove once UpdateTurnCount is called externally
        }
    }
}
