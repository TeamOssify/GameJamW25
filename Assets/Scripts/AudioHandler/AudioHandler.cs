using UnityEngine;

public class PlayAudio : MonoBehaviour {

    public AudioSource titleIntro;
    public AudioSource titleTheme;

    private void Start() {

        // Length of intro
        double duration = (double)titleIntro.clip.samples / titleIntro.clip.frequency;
        double start = AudioSettings.dspTime + 0.1;
        Debug.Log("Intro length" + duration);

        titleIntro.PlayScheduled(start);
        titleTheme.PlayScheduled(start + duration);
    }
}