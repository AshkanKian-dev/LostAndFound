using UnityEngine;
using System.Collections.Generic;

public class NPCDialogue : MonoBehaviour, IInteractable
{
    [Header("NPC Dialogue Settings")]
    public List<string> wordsToTeach = new List<string>();
    [TextArea] public string originalDialogue = "zok! mivor talun?";

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
        }
        else
        {
            string translated = LanguageManager.Instance.TranslateSentence(originalDialogue);
            DialogueUIManager.Instance.ShowDialogue(translated);
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
