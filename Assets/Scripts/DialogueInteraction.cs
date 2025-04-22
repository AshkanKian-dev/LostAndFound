// DialogueInteraction.cs
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DialogueInteraction : MonoBehaviour, IInteractable
{
    [Header("Conversation Lines (in original language)")]
    [TextArea] public string[] originalLines;

    [Header("Translated (English) Lines")]
    [TextArea] public string[] translatedLines;

    [Header("Portrait (optional)")]
    public Sprite portrait;

    [Header("Language Unlock (optional)")]
    public LanguageData requiredLanguage;

    [Header("Quest Settings")]
    [Tooltip("The LearnableObject (e.g. your tomb) to unlock")]
    public LearnableObject targetTomb;
    [Tooltip("Where to spawn the quest indicator ring")]
    public Transform questLocation;

    [Header("Quest Message")]
    [TextArea]
    [Tooltip("What to show when the tomb first opens")]
    public string questMessage = "The tomb is now open! Go learn its secrets.";

    bool questStarted = false;
    bool inConversation = false;
    bool hintShown = false;
    int currentLine = 0;

    public void Interact()
    {
        // ─── 1) on very first F → unlock + ring + show latin + custom toast ───
        if (!questStarted)
        {
            questStarted = true;

            if (targetTomb != null)
                targetTomb.Unlock();

            if (questLocation != null)
                QuestManager.Instance.ShowQuestArea(questLocation.position);

            // show the first Latin line
            if (originalLines.Length > 0)
            {
                DialogueUIManager.Instance.ShowDialogue(
                    originalLines[currentLine],
                    portrait
                );
                // advance so next time we cycle to the next line
                currentLine = (currentLine + 1) % originalLines.Length;
                inConversation = true;
            }

            // show your custom center‑screen toast
            LanguageLearnedUI.Instance.Show(
                questMessage,
                requireInput: false,
                autoHideSec: 5f
            );

            return;
        }

        // ─── 2) after that, carry on with your usual dialogue logic ───
        bool knows = requiredLanguage == null ||
                     LanguageManager.Instance.KnowsLanguage(requiredLanguage.languageName);

        if (!knows)
        {
            // speak Latin
            DialogueUIManager.Instance.ShowDialogue(
                originalLines[currentLine],
                portrait
            );
            currentLine = (currentLine + 1) % originalLines.Length;

            if (!hintShown)
            {
                LanguageLearnedUI.Instance.Show(
                    $"" +
                    $"",
                    requireInput: false,
                    autoHideSec: 4f
                );
                hintShown = true;
            }
            return;
        }

        // now that you know the language, show English lines
        if (!inConversation)
        {
            inConversation = true;
            currentLine = 0;
            DialogueUIManager.Instance.ShowDialogue(
                translatedLines[currentLine],
                portrait
            );
        }
        else if (currentLine < translatedLines.Length - 1)
        {
            currentLine++;
            DialogueUIManager.Instance.ShowDialogue(
                translatedLines[currentLine],
                portrait
            );
        }
        else
        {
            EndConversation();
        }
    }

    void EndConversation()
    {
        DialogueUIManager.Instance.HideDialogue();
        inConversation = false;
    }

    void OnDisable()
    {
        if (DialogueUIManager.Instance != null && DialogueUIManager.Instance.IsDialogueActive())
            DialogueUIManager.Instance.HideDialogue();
        inConversation = false;
    }
}
