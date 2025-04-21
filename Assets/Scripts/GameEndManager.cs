using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEndManager : MonoBehaviour
{
    public static GameEndManager Instance;

    [Header("Exit Setup")]
    [Tooltip("The GameObject (e.g. passport desk collider) to enable once you've learned all languages")]
    public GameObject exitObject;

    [Header("On‑Unlock Message")]
    [TextArea]
    [Tooltip("What text to show when the exit unlocks")]
    public string unlockMessage = "Congratulations! You may now collect your passport.";

    [Tooltip("Should the player have to press F to dismiss this message?")]
    public bool requireInputToDismiss = true;

    [Tooltip("If not waiting for input, how long before auto‑hide?")]
    public float autoHideSeconds = 3f;

    [Header("Ending Cutscene (Optional)")]
    public Sprite endingSprite;
    [TextArea]
    public string endingText = "Thank you for playing!";
    public bool playEndingCutscene = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
            return;
        }

        // hide the exit until it's unlocked
        if (exitObject != null)
            exitObject.SetActive(false);
    }

    /// <summary>
    /// Called by LanguageManager once you've learned every language.
    /// </summary>
    public void UnlockExit()
    {
        Debug.Log("GameEndManager.UnlockExit()");

        // 1) Enable the exit object (e.g. passport desk collider)
        if (exitObject != null)
            exitObject.SetActive(true);

        // 2) Show your custom on‑screen toast
        LanguageLearnedUI.Instance.Show(
            unlockMessage,
            requireInput: requireInputToDismiss,
            autoHideSec: autoHideSeconds,
            onDone: () =>
            {
                // 3) Optionally trigger the final cutscene once the player dismisses it
                if (playEndingCutscene)
                    TriggerEndingCutscene();
            }
        );
    }

    /// <summary>
    /// If you want to drive into a final cutscene when the player actually uses the exit.
    /// </summary>
    public void TriggerEndingCutscene()
    {
        if (CutsceneManager.Instance == null)
        {
            Debug.LogWarning("No CutsceneManager found in scene!");
            // fallback: just go back to menu
            SceneManager.LoadScene("MainMenu");
            return;
        }

        CutsceneManager.Instance.PlayCutscene(
            endingSprite,
            endingText,
            () => SceneManager.LoadScene("MainMenu")
        );
    }
}
