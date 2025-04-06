using UnityEngine;
using System.Collections.Generic;

public class NPCDialogue : MonoBehaviour, IInteractable
{
    // List of words the NPC can teach (if applicable)
    public List<string> wordsToTeach = new List<string>();

    // Example dialogue text
    private string originalDialogue = "zok! mivor talun?";

    void Start()
    {
        DisplayDialogue();
    }

    // Called when the player interacts with this NPC
    public void Interact()
    {
        ShowDialogue();
    }

    // Displays the dialogue (for now just logs to the Console)
    public void ShowDialogue()
    {
        string translated = LanguageManager.Instance.TranslateSentence(originalDialogue);
        DialogueUIManager.Instance.ShowDialogue(translated);
    }

    // Optionally, this method can teach words to the player
    public void TeachPlayerWords()
    {
        if (LanguageManager.Instance != null)
        {
            foreach (string word in wordsToTeach)
            {
                LanguageManager.Instance.LearnWord(word);
            }
            wordsToTeach.Clear(); // Prevent re-teaching
            DisplayDialogue();
        }
    }

    void DisplayDialogue()
    {
        Debug.Log("NPC dialogue: " + originalDialogue);
    }

    // This method will be called when the player exits the NPC's collider.
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            DialogueUIManager.Instance.HideDialogue();
        }
    }
}
