using UnityEngine;
using System.Collections.Generic;

public class NPCDialogue : MonoBehaviour, IInteractable
{
    [Header("NPC Dialogue Settings")]
    public List<string> wordsToTeach = new List<string>();
    [TextArea] public string originalDialogue = "salve quid agis?"; // Latin/Temple
    [TextArea] public string translatedDialogue = "Hello, how are you?"; // English

    [Header("Language Requirement")]
    public LanguageData requiredLanguage; // Now using ScriptableObject reference

    private bool playerInRange = false;
    private bool hasTaughtWords = false;

    public void Interact()
    {
        if (!playerInRange)
        {
            Debug.Log("Player not in range of NPC: " + gameObject.name);
            return;
        }

        Debug.Log("Interacting with NPC: " + gameObject.name);

        if (DialogueUIManager.Instance.IsDialogueActive())
        {
            DialogueUIManager.Instance.HideDialogue();
            return;
        }

        // Show translated dialogue if the player knows the language
        if (requiredLanguage != null)
        {
            if (LanguageManager.Instance.KnowsLanguage(requiredLanguage.languageName))
            {
                DialogueUIManager.Instance.ShowDialogue(translatedDialogue);
            }
            else
            {
                DialogueUIManager.Instance.ShowDialogue(originalDialogue);
            }
        }
        else
        {
            // No language restriction
            DialogueUIManager.Instance.ShowDialogue(translatedDialogue);
        }

        if (!hasTaughtWords)
        {
            TeachPlayerWords();
            hasTaughtWords = true;
        }
    }

    private void TeachPlayerWords()
    {
        foreach (string word in wordsToTeach)
        {
            Debug.Log("Trying to learn word: " + word);
            LanguageManager.Instance.LearnWord(word);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Player entered range of NPC: " + gameObject.name);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("Player exited range of NPC: " + gameObject.name);
            DialogueUIManager.Instance.HideDialogue();
        }
    }
}
