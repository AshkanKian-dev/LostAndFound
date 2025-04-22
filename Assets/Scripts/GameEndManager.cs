using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEndManager : MonoBehaviour
{
    public static GameEndManager Instance;

    [Header("Exit Setup")]
    [Tooltip("The GameObject (e.g. your boat) to enable once you've learned all languages")]
    public GameObject exitObject;

    [Header("On‑Unlock Message")]
    [TextArea]
    public string unlockMessage = "The path is clear! Press F to board.";
    public bool requireInputToDismiss = true;
    public float autoHideSeconds = 3f;

    [Header("Audio Sources")]
    [Tooltip("Your normal gameplay/music AudioSource")]
    public AudioSource mainAudioSource;
    [Tooltip("AudioSource for your cutscene music")]
    public AudioSource cutsceneAudioSource;

    [Header("Ending Cutscene (Optional)")]
    public Sprite endingSprite;
    [TextArea]
    public string endingText = "Thank you for playing!";
    public AudioClip endingMusic;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            if (exitObject) exitObject.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        // if you didn't set up any win‐list, let them board immediately
        var lm = LanguageManager.Instance;
        bool hasWinList = lm != null
                          && lm.allLanguages != null
                          && lm.allLanguages.Length > 0;
        if (exitObject != null)
            exitObject.SetActive(!hasWinList);
    }

    public void UnlockExit()
    {
        if (exitObject) exitObject.SetActive(true);
        LanguageLearnedUI.Instance.Show(
            unlockMessage,
            requireInputToDismiss,
            autoHideSeconds
        );
    }

    public void TriggerEndingCutscene()
    {
        // fallback if you forgot to add a CutsceneManager to the scene
        if (CutsceneManager.Instance == null)
        {
            Debug.LogWarning("No CutsceneManager found – loading MainMenu immediately.");
            SceneManager.LoadScene("MainMenu");
            return;
        }

        var cm = CutsceneManager.Instance;

        // 1) configure your audio sources on the CutsceneManager
        if (mainAudioSource != null)
            cm.mainAudioSource = mainAudioSource;

        if (cutsceneAudioSource != null)
            cm.cutsceneAudioSource = cutsceneAudioSource;

        // 2) configure the clip
        if (endingMusic != null)
            cm.endingMusic = endingMusic;

        // 3) configure the visuals
        if (endingSprite != null)
            cm.cutsceneImage.sprite = endingSprite;

        cm.cutsceneText.text = endingText;

        // 4) play it
        cm.PlayCutscene(endingSprite, endingText);
    }
}
