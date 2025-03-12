using UnityEngine;
using TMPro; // For UI text

public class NPCDialogue : MonoBehaviour, IInteractable
{
    public List<string> wordsToTeach = new List<string>(); // List of words NPC can teach
    private string originalDialogue = "zok! mivor talun?"; // Default dialogue before learning

    public void Interact()
    {
        TeachPlayerWords();
    }

    void Start()
    {
        LanguageManager.Instance.WordLearned += (word, translation) => DisplayDialogue();
        DisplayDialogue();
    }

    void DisplayDialogue()
    {
        if (LanguageManager.Instance != null)
        {
            dialogueText.text = LanguageManager.Instance.TranslateSentence(originalDialogue);
        }
    }

    public void TeachPlayerWord(string word)
    {
        foreach (string word in wordsToTeach)
        {
            LanguageManager.Instance.LearnWord(word); // Learn all words in the list
        }
        wordsToTeach.Clear(); // Prevent learning again from the same NPC
        DisplayDialogue(); // Update dialogue to reflect new understanding
    }
}
