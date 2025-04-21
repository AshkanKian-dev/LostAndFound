using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class LearnableObject : MonoBehaviour, IInteractable
{
    [Header("Custom Learn Popup")]
    [TextArea(2, 5)]
    public string learnMessage = "You learned the {0} language!";

    [Header("Language Settings")]
    public LanguageData languageToLearn;
    public bool IsReusable = false;

    [Header("Lock Settings")]
    [Tooltip("Starts locked: cannot be interacted until unlocked by the NPC")]
    public bool isLocked = true;

    bool hasBeenLearned = false;
    Collider2D col;

    void Awake()
    {
        col = GetComponent<Collider2D>();
        if (col != null) col.enabled = !isLocked;
    }

    /// <summary>
    /// Called from DialogueInteraction on first F‑press.
    /// </summary>
    public void Unlock()
    {
        isLocked = false;
        if (col != null) col.enabled = true;
    }

    public void Interact()
    {
        if (isLocked) return;
        if (hasBeenLearned && !IsReusable) return;
        if (languageToLearn == null || LanguageManager.Instance == null)
        {
            Debug.LogWarning("Missing LanguageData or LanguageManager");
            return;
        }

        // Teach the language
        LanguageManager.Instance.LearnLanguage(languageToLearn.languageName);

        // Show your custom popup for exactly 5 seconds
        string msg = string.Format(learnMessage, languageToLearn.languageName);
        LanguageLearnedUI.Instance.Show(
            msg,
            requireInput: false,  // auto‑hide
            autoHideSec: 5f
        );

        // Clear the quest indicator ring
        QuestManager.Instance?.ClearQuestArea();

        hasBeenLearned = true;
    }

    // (optional) for your “Press F to learn X” prompt
    public string GetInteractText()
    {
        return $"Press F to learn {languageToLearn.languageName}";
    }
}
