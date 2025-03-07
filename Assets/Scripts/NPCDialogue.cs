using UnityEngine;
using TMPro; // For UI text

public class NPCDialogue : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    private string originalDialogue = "zok! mivor talun?";

    void Start()
    {
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
        LanguageManager.Instance.LearnWord(word);
        DisplayDialogue(); // Update text after learning
    }
}
