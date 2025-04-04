using UnityEngine;
using TMPro;

public class NPCDialogue : MonoBehaviour
{
    [SerializeField] private string[] dialogueLines;
    [SerializeField] private TMP_Text dialogueText;  // Reference to your dialogue UI box
    private int currentLine = 0;

    public void Interact()
    {
        TeachPlayerWords();
    }


    public void TeachPlayerWords()
    {
        LanguageManager.Instance.LearnRandomWord();
        ShowDialogueLine();  // After learning, show the next line
    }

    public void ShowDialogueLine()
    {
        if (currentLine >= dialogueLines.Length)
        {
            currentLine = 0;
            dialogueText.text = "";
            return;
        }

        string processedLine = ProcessLine(dialogueLines[currentLine]);
        dialogueText.text = processedLine;
        currentLine++;
    }

    private string ProcessLine(string line)
    {
        string[] words = line.Split(' ');
        for (int i = 0; i < words.Length; i++)
        {
            string cleanWord = words[i].TrimEnd('.', ',', '!', '?').ToLower();
            if (!LanguageManager.Instance.learnedWords.Contains(cleanWord))
            {
                words[i] = GenerateGibberish(words[i]);
            }
        }

        return string.Join(" ", words);
    }

    private string GenerateGibberish(string originalWord)
    {
        string gibberish = "";
        int length = originalWord.Length;
        string charset = "#@%!?&*";

        for (int i = 0; i < length; i++)
        {
            gibberish += charset[Random.Range(0, charset.Length)];
        }

        // Preserve punctuation
        char lastChar = originalWord[originalWord.Length - 1];
        if (!char.IsLetterOrDigit(lastChar))
        {
            gibberish = gibberish.Substring(0, gibberish.Length - 1) + lastChar;
        }

        return gibberish;
    }
}
