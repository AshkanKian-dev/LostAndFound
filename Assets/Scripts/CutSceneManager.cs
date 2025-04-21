using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager Instance;

    [Header("UI")]
    public GameObject cutscenePanel;     // full‑screen panel (must have a CanvasGroup!)
    public Image cutsceneImage;       // your ending sprite
    public TextMeshProUGUI cutsceneText; // your ending text

    [Header("Audio (optional)")]
    public AudioSource audioSource;      // drag in an AudioSource
    public AudioClip endingMusic;      // your ending music clip

    [Header("Timing")]
    public float fadeToBlackDuration = 2f;
    public float holdBlackDuration = 1f;
    public float fadeFromBlackDuration = 2f;

    private CanvasGroup panelGroup;
    private Action onComplete;
    private bool active;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        // cache the CanvasGroup for fading
        panelGroup = cutscenePanel.GetComponent<CanvasGroup>();
        if (panelGroup == null)
        {
            Debug.LogError("CutsceneManager: cutscenePanel needs a CanvasGroup!");
        }

        // hide to start
        cutscenePanel.SetActive(false);
        panelGroup.alpha = 0f;
    }

    void Update()
    {
        // allow manual skip if you like (press F)
        if (active && Input.GetKeyDown(KeyCode.F))
        {
            StopAllCoroutines();
            EndCutscene();
        }
    }

    /// <summary>
    /// Plays a fancy fade‑to‑black → reveal cutscene.
    /// </summary>
    public void PlayCutscene(Sprite img, string text, Action onDone = null)
    {
        if (cutsceneImage != null) cutsceneImage.sprite = img;
        if (cutsceneText != null) cutsceneText.text = text;
        if (cutscenePanel != null) cutscenePanel.SetActive(true);

        onComplete = onDone;
        active = true;

        // freeze gameplay
        Time.timeScale = 0f;

        // start the sequence
        StartCoroutine(CutsceneRoutine());
    }

    private IEnumerator CutsceneRoutine()
    {
        // 1) Fade to black
        yield return FadeCanvasGroup(panelGroup, 0f, 1f, fadeToBlackDuration);

        // 2) Hold full black
        yield return new WaitForSecondsRealtime(holdBlackDuration);

        // 3) Play ending music if provided
        if (audioSource != null && endingMusic != null)
        {
            audioSource.clip = endingMusic;
            audioSource.Play();
        }

        // 4) Fade back in from black to reveal your image/text
        yield return FadeCanvasGroup(panelGroup, 1f, 0f, fadeFromBlackDuration);

        // 5) If you want to hold the final image/music for its length:
        if (endingMusic != null && audioSource != null)
            yield return new WaitForSecondsRealtime(endingMusic.length);

        // 6) Finish
        EndCutscene();
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup cg, float from, float to, float duration)
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            cg.alpha = Mathf.Lerp(from, to, t / duration);
            yield return null;
        }
        cg.alpha = to;
    }

    private void EndCutscene()
    {
        // hide panel
        if (cutscenePanel != null) cutscenePanel.SetActive(false);

        active = false;
        // un‑pause
        Time.timeScale = 1f;
        // fire callback
        onComplete?.Invoke();
    }
}
