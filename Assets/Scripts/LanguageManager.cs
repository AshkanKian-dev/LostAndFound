using System.Collections.Generic;
using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager Instance;

    // Store the word dictionary with just English words
    public Dictionary<string, string> wordDictionary = new Dictionary<string, string>();
    public HashSet<string> learnedWords = new HashSet<string>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        // Load words from the JSON file
        LoadWordLibrary();
    }

    // Load the word library from the JSON file located in the Resources folder
    private void LoadWordLibrary()
    {
        TextAsset wordLibraryJson = Resources.Load<TextAsset>("wordLibrary");

        if (wordLibraryJson != null)
        {
            // Deserialize the JSON into the dictionary
            wordDictionary = JsonUtility.FromJson<WordLibrary>(wordLibraryJson.ToString()).wordDictionary;
            Debug.Log("[LanguageManager] Word library loaded successfully.");
        }
        else
        {
            Debug.LogError("[LanguageManager] Failed to load word library JSON file.");
        }
    }

    // Function to learn a word
    public void LearnWord(string wordKey)
    {
        if (!wordDictionary.ContainsKey(wordKey))
        {
            Debug.LogError($"[LanguageManager] Word '{wordKey}' not found in dictionary!");
            return;
        }

        if (learnedWords.Add(wordKey))
        {
            Debug.Log($"[LanguageManager] You learned the word: {wordKey}.");
        }
        else
        {
            Debug.Log($"[LanguageManager] Already learned the word: {wordKey}");
        }
    }

    // Check if the word is learned
    public bool IsWordLearned(string word)
    {
        return learnedWords.Contains(word);
    }

    // Learn a random word
    public bool LearnRandomWord()
    {
        List<string> unlearned = new List<string>();

        foreach (var entry in wordDictionary)
        {
            if (!learnedWords.Contains(entry.Key))
                unlearned.Add(entry.Key);
        }

        if (unlearned.Count > 0)
        {
            string randomWord = unlearned[Random.Range(0, unlearned.Count)];
            LearnWord(randomWord);
            return true;
        }

        Debug.Log("No more words to learn.");
        return false;
    }

    // Get the list of learned words
    public List<string> GetLearnedWords()
    {
        return new List<string>(learnedWords);
    }

    // Get the translation of a word (English word if learned, gibberish if not)
    public string GetTranslation(string wordKey)
    {
        if (learnedWords.Contains(wordKey))
            return wordKey; // If the word is learned, return it in English.

        return GenerateGibberish(wordKey); // If not learned, return gibberish
    }

    // Helper function to generate gibberish for unknown words
    private string GenerateGibberish(string word)
    {
        string gibberish = "";
        string charset = "#@%!?&*"; // Characters used for gibberish
        for (int i = 0; i < word.Length; i++)
        {
            gibberish += charset[Random.Range(0, charset.Length)];
        }

        return gibberish;
    }
}

// Helper class to hold the word dictionary for JSON parsing
[System.Serializable]
public class WordLibrary
{
    public Dictionary<string, string> wordDictionary = new Dictionary<string, string>();
}
