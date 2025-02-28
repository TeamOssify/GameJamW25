using Eflatun.SceneReference;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class TitleScreenController : MonoBehaviour {
    [SerializeField]
    private SceneReference mainMenu;

    // Setup events
    public void Start() {
        var playBtn = GetComponent<UIDocument>().rootVisualElement.Q<Button>("PlayButton");
        var quitBtn = GetComponent<UIDocument>().rootVisualElement.Q<Button>("QuitButton");

        playBtn.clicked += onPlay;
        quitBtn.clicked += onQuit;
    }

    private void onPlay() {
        SceneManager.LoadScene(mainMenu.BuildIndex);
    }

    private void onQuit() {
        // Doesn't work in editor, this is intended functionality (ig)
        Application.Quit();
    }
}
