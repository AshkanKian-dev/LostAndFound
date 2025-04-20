using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DialogueInteraction : MonoBehaviour, IInteractable
{
    [Header("Raw (Latin) Lines")]
    [TextArea] public string[] originalLines;

    [Header("Translated (English) Lines")]
    [TextArea] public string[] translatedLines;

    [Header("Portrait")]
    [Tooltip("Character headshot for VN-style dialogue.")]
    public Sprite portrait;

    [Header("Language Unlock")]
    [Tooltip("The language this NPC speaks (learn it via the tome).")]
    public LanguageData requiredLanguage;

    [Header("Quest Area Settings")]
    [Tooltip("Drag here the empty GameObject positioned at the tome’s location.")]
    public Transform questLocation;

    private int currentLine = 0;
    private bool inConversation = false;
    private bool hintShown = false;

    public void Interact()
    {
        // Do we already know this language?
        bool knows = (requiredLanguage == null)
                     || LanguageManager.Instance.KnowsLanguage(requiredLanguage.languageName);

        // If not known yet, show raw line + one‑time hint + area indicator
        if (!knows)
        {
            ShowOriginalLine();

            if (!hintShown)
            {
                // 1) tell them where to look
                LanguageLearnedUI.Instance.Show(
                    $"You can’t understand {requiredLanguage.languageName}! " +
                    $"Find the {requiredLanguage.languageName} tome to learn it."
                );

                // 2) spawn the ring at questLocation
                if (questLocation != null && QuestManager.Instance != null)
                    QuestManager.Instance.ShowQuestArea(questLocation.position);

                hintShown = true;
            }
            return;
        }

        // Once learned, clear the indicator and run a full conversation
        if (hintShown && QuestManager.Instance != null)
            QuestManager.Instance.ClearQuestArea();

        if (!inConversation)
        {
            inConversation = true;
            currentLine = 0;
            ShowTranslatedLine();
        }
        else if (currentLine < translatedLines.Length - 1)
        {
            currentLine++;
            ShowTranslatedLine();
        }
        else
        {
            EndConversation();
        }
    }

    private void ShowOriginalLine()
    {
        if (originalLines.Length == 0) return;
        DialogueUIManager.Instance.ShowDialogue(originalLines[currentLine], portrait);
        currentLine = (currentLine + 1) % originalLines.Length;
    }

    private void ShowTranslatedLine()
    {
        if (translatedLines.Length == 0) return;
        DialogueUIManager.Instance.ShowDialogue(translatedLines[currentLine], portrait);
    }

    private void EndConversation()
    {
        DialogueUIManager.Instance.HideDialogue();
        inConversation = false;
    }

    private void OnDisable()
    {
        // Hide the UI if we get turned off mid‑chat
        if (DialogueUIManager.Instance != null
         && DialogueUIManager.Instance.IsDialogueActive())
        {
            DialogueUIManager.Instance.HideDialogue();
        }
        inConversation = false;
    }
}
