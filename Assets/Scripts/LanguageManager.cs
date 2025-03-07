using System.Collections.Generic;
using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager Instance;

    private Dictionary<string, string> wordDictionary = new Dictionary<string, string>()
    {
        { "zok", "hello" },
        { "mivor", "friend" },
        { "talun", "food" },
        { "varak", "danger" }
    };

    private Dictionary<string, int> wordExposure = new Dictionary<string, int>(); // Tracks exposure count
    private HashSet<string> learnedWords = new HashSet<string>();

    public delegate void OnWordLearned(string word, string translation);
    public event OnWordLearned WordLearned; // Event for UI updates

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Translates a given sentence, revealing words based on learning progress.
    /// </summary>
    public string TranslateSentence(string sentence)
    {
        string[] words = sentence.Split(' ');
        for (int i = 0; i < words.Length; i++)
        {
            if (wordDictionary.ContainsKey(words[i]))
            {
                if (learnedWords.Contains(words[i]))
                {
                    words[i] = wordDictionary[words[i]]; // Fully translated
                }
                else
                {
                    words[i] = RevealPartialWord(words[i]); // Partial translation
                }
            }
        }
        return string.Join(" ", words);
    }

    /// <summary>
    /// Reveals parts of a word based on exposure count.
    /// </summary>
    private string RevealPartialWord(string word)
    {
        if (!wordExposure.ContainsKey(word))
            wordExposure[word] = 0;

        wordExposure[word]++;

        int revealCount = Mathf.Clamp(wordExposure[word], 1, wordDictionary[word].Length);
        char[] revealedWord = GenerateConsistentGibberish(word).ToCharArray();

        for (int i = 0; i < revealCount; i++)
        {
            revealedWord[i] = wordDictionary[word][i]; // Gradually reveal letters
        }

        return new string(revealedWord);
    }

    /// <summary>
    /// Generates consistent placeholder text to represent an unknown word.
    /// </summary>
    private string GenerateConsistentGibberish(string word)
    {
        const string letters = "abcdefghijklmnopqrstuvwxyz";
        char[] gibberish = new char[word.Length];

        for (int i = 0; i < word.Length; i++)
        {
            gibberish[i] = letters[(word[i] * 3) % letters.Length]; // Generates consistent gibberish
        }

        return new string(gibberish);
    }

    /// <summary>
    /// Marks a word as learned and triggers UI updates.
    /// </summary>
    public void LearnWord(string word)
    {
        if (wordDictionary.ContainsKey(word) && !learnedWords.Contains(word))
        {
            learnedWords.Add(word);
            wordExposure.Remove(word); // Stop tracking exposure

            WordLearned?.Invoke(word, wordDictionary[word]); // Notify UI
        }
    }

    /// <summary>
    /// Instantly learns a word when interacting with labeled objects.
    /// </summary>
    public void LearnWordFromItem(string word)
    {
        LearnWord(word);
    }

    /// <summary>
    /// Returns a list of all learned words for the Journal UI.
    /// </summary>
    public List<string> GetLearnedWords()
    {
        return new List<string>(learnedWords);
    }

    /// <summary>
    /// Returns the translation of a word if known; otherwise, returns ???.
    /// </summary>
    public string GetTranslation(string word)
    {
        return wordDictionary.ContainsKey(word) ? wordDictionary[word] : "???";
    }
}
