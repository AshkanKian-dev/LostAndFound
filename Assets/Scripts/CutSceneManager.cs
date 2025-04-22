using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager Instance;

    [Header("UI (Panel with CanvasGroup)")]
    public CanvasGroup panelGroup;          // Fullscreen overlay panel
    public Image cutsceneImage;             // Cutscene sprite
    public TextMeshProUGUI cutsceneText;    // Cutscene text

    [Header("Audio")]
    public AudioSource mainAudioSource;     // Gameplay BGM
    public AudioSource cutsceneAudioSource; // Cutscene music source
    public AudioClip endingMusic;           // Cutscene soundtrack

    [Header("Timing (seconds)")]
    public float fadeDuration = 1f;         // Fade in/out duration
    public float holdDuration = 5f;         // How long to show cutscene

    private Coroutine cutsceneRoutine;

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Ensure panel starts hidden
        if (panelGroup != null)
        {
            panelGroup.alpha = 0f;
            panelGroup.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Begins the cutscene: fade in, hold, fade out, then load MainMenu.
    /// </summary>
    public void PlayCutscene(Sprite img, string text)
    {
        // Set content
        if (cutsceneImage != null) cutsceneImage.sprite = img;
        if (cutsceneText != null) cutsceneText.text = text;

        // Show panel and pause gameplay
        if (panelGroup != null)
        {
            panelGroup.gameObject.SetActive(true);
            panelGroup.alpha = 0f;
        }
        Time.timeScale = 0f;

        // Switch audio
        mainAudioSource?.Stop();
        if (cutsceneAudioSource != null && endingMusic != null)
        {
            cutsceneAudioSource.clip = endingMusic;
            cutsceneAudioSource.volume = 1f;
            cutsceneAudioSource.Play();
        }

        // Start sequence
        cutsceneRoutine = StartCoroutine(CutsceneSequence());
    }

    private IEnumerator CutsceneSequence()
    {
        // Fade in
        yield return Fade(panelGroup, 0f, 1f, fadeDuration);

        // Hold cutscene
        yield return new WaitForSecondsRealtime(holdDuration);

        // Fade out
        yield return Fade(panelGroup, 1f, 0f, fadeDuration);

        // Restore time
        Time.timeScale = 1f;

        // Hide panel
        if (panelGroup != null)
            panelGroup.gameObject.SetActive(false);

        // Load main menu
        SceneManager.LoadScene("MainMenu");
    }

    private IEnumerator Fade(CanvasGroup cg, float start, float end, float duration)
    {
        if (cg == null) yield break;
        float elapsed = 0f;
        cg.alpha = start;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            cg.alpha = Mathf.Lerp(start, end, elapsed / duration);
            yield return null;
        }
        cg.alpha = end;
    }

    /// <summary>
    /// Skip the cutscene, immediately return to menu.
    /// </summary>
    public void SkipCutscene()
    {
        if (cutsceneRoutine != null)
            StopCoroutine(cutsceneRoutine);

        cutsceneAudioSource?.Stop();
        Time.timeScale = 1f;

        if (panelGroup != null)
        {
            panelGroup.alpha = 0f;
            panelGroup.gameObject.SetActive(false);
        }

        SceneManager.LoadScene("MainMenu");
    }
}