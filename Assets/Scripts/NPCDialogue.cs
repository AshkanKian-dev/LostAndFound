using UnityEngine;
using System.Collections.Generic;

public class NPCDialogue : MonoBehaviour
{
    // List of words the NPC can teach
    public List<string> wordsToTeach = new List<string>();

    // An example default dialogue string (optional)
    private string originalDialogue = "zok! mivor talun?";

    void Start()
    {
        // If your LanguageManager has an event for when a word is learned, you can subscribe here
        if (LanguageManager.Instance != null)
        {
            LanguageManager.Instance.WordLearned += (word, translation) => DisplayDialogue();
        }

        DisplayDialogue();
    }

    void DisplayDialogue()
    {
        if (LanguageManager.Instance != null)
        {
            // Example: Print the translated sentence to the console (or update a UI text)
            Debug.Log("NPC says: " + LanguageManager.Instance.TranslateSentence(originalDialogue));
        }
        else
        {
            Debug.Log("NPC says: " + originalDialogue);
        }
    }

    // Teaches all words in wordsToTeach to the player
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
}
