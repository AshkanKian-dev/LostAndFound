using System;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainMenuPanel;    // your regular menu buttons
    public GameObject optionsPanel;     // your credits/options panel

    [Header("Intro Cutscene UI (must have a CanvasGroup)")]
    public GameObject cutscenePanel;
    public Image cutsceneImage;      // the big black‑fade image
    public TextMeshProUGUI cutsceneText;  // the intro text
    public AudioSource cutsceneAudio;  // AudioSource on same panel
    public AudioClip cutsceneMusic;  // intro music clip

    [Header("Cutscene Settings")]
    public float fadeToBlackDuration = 1f;
    public float holdBlackDuration = 1.5f;
    public float fadeFromBlackDuration = 1f;

    [Header("Cutscene Content")]
    public Sprite introSprite;   // your splash/logo
    [TextArea] public string introCopy;  // your on‑screen line

    private CanvasGroup _cg;
    private bool _playingCutscene = false;

    void Start()
    {
        // menu/credits setup
        mainMenuPanel.SetActive(true);
        optionsPanel.SetActive(false);

        // cutscene setup
        _cg = cutscenePanel.GetComponent<CanvasGroup>();
        if (_cg == null)
            Debug.LogError("CutscenePanel needs a CanvasGroup!");
        cutscenePanel.SetActive(false);
        _cg.alpha = 0f;
    }

    // --------------------------------------------------
    // UI Button Hooks
    // --------------------------------------------------

    /// <summary>
    /// Hook your “Play” button here.
    /// </summary>
    public void PlayGame()
    {
        // hide the menu buttons
        mainMenuPanel.SetActive(false);

        // configure cutscene
        cutsceneImage.sprite = introSprite;
        cutsceneText.text = introCopy;
        if (cutsceneAudio != null && cutsceneMusic != null)
            cutsceneAudio.clip = cutsceneMusic;

        // start it
        cutscenePanel.SetActive(true);
        _playingCutscene = true;
        Time.timeScale = 0f;  // freeze gameplay underneath
        StartCoroutine(CutsceneRoutine(() =>
        {
            // once done, unfreeze and load next scene
            Time.timeScale = 1f;
            SceneManager.LoadScene("MainWorld");
        }));
    }

    /// <summary>
    /// Hook your “Options/Credits” button here.
    /// </summary>
    public void OpenOptions()
    {
        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    /// <summary>
    /// Hook your “Back” button inside options/credits.
    /// </summary>
    public void CloseOptions()
    {
        optionsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    /// <summary>
    /// Hook your “Quit” button here.
    /// </summary>
    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    void Update()
    {
        // allow pressing F to skip the intro
        if (_playingCutscene && Input.GetKeyDown(KeyCode.F))
        {
            StopAllCoroutines();
            if (cutsceneAudio != null) cutsceneAudio.Stop();
            EndCutscene();
        }
    }

    // --------------------------------------------------
    // Cutscene Coroutine
    // --------------------------------------------------

    private IEnumerator CutsceneRoutine(Action onDone)
    {
        // fade *to* black
        yield return Fade(0f, 1f, fadeToBlackDuration);

        // hold full black
        yield return new WaitForSecondsRealtime(holdBlackDuration);

        // play music
        if (cutsceneAudio != null && cutsceneMusic != null)
            cutsceneAudio.Play();

        // fade *from* black
        yield return Fade(1f, 0f, fadeFromBlackDuration);

        // optional: wait for music to finish
        if (cutsceneAudio != null && cutsceneMusic != null)
            yield return new WaitForSecondsRealtime(cutsceneMusic.length);

        EndCutscene();
        onDone?.Invoke();
    }

    private IEnumerator Fade(float from, float to, float duration)
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            _cg.alpha = Mathf.Lerp(from, to, t / duration);
            yield return null;
        }
        _cg.alpha = to;
    }

    private void EndCutscene()
    {
        cutscenePanel.SetActive(false);
        _playingCutscene = false;
    }
}
