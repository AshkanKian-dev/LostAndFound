using System.Collections.Generic;
using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager Instance;

    public Dictionary<string, string> wordDictionary = new Dictionary<string, string>();
    public HashSet<string> learnedWords = new HashSet<string>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        // Example initialization (you can load this from JSON or ScriptableObject later)
        wordDictionary.Add("apple", "Apple");
        wordDictionary.Add("tree", "Tree");
        wordDictionary.Add("water", "Water");
        wordDictionary.Add("friend", "Friend");
        wordDictionary.Add("danger", "Danger");
        // Add more as needed...
    }

    public void LearnWord(string wordKey)
    {
        if (!learnedWords.Contains(wordKey) && wordDictionary.ContainsKey(wordKey))
        {
            learnedWords.Add(wordKey);
            Debug.Log($"You learned the word: {wordKey} = {wordDictionary[wordKey]}");
        }
        else
        {
            Debug.Log($"Already learned or unknown word: {wordKey}");
        }
    }

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
    public List<string> GetLearnedWords()
    {
        return new List<string>(learnedWords);
    }
    public string GetTranslation(string wordKey)
    {
        if (wordDictionary.ContainsKey(wordKey))
            return wordDictionary[wordKey];

        return "(Unknown)";
    }

}
