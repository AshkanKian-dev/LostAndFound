using UnityEngine;
using System.Collections.Generic;

public class NPCDialogue : MonoBehaviour, IInteractable
{
    [Header("NPC Dialogue Settings")]
    public List<string> wordsToTeach = new List<string>();
    [TextArea] public string originalDialogue = "salve quid agis?";
    [TextArea] public string translatedDialogue = "Hello, how are you?";

    [Header("Language Requirement")]
    public LanguageData requiredLanguage;

    private bool hasTaughtWords = false;

    public void Interact()
    {
        Debug.Log("Interacting with NPC: " + gameObject.name);

        // toggle dialogue UI
        if (DialogueUIManager.Instance.IsDialogueActive())
        {
            DialogueUIManager.Instance.HideDialogue();
            return;
        }

        // show appropriate text
        bool knows = requiredLanguage != null
                     && LanguageManager.Instance.KnowsLanguage(requiredLanguage.languageName);

        DialogueUIManager.Instance.ShowDialogue(knows ? translatedDialogue : originalDialogue);

        // teach new words only once
        if (!hasTaughtWords)
        {
            foreach (var w in wordsToTeach)
                LanguageManager.Instance.LearnWord(w);
            hasTaughtWords = true;
        }
    }
}
