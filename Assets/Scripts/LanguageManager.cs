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
        { "varak", "danger" },
        { "shari", "welcome" },
        { "tavok", "house" }
    };

    private Dictionary<string, int> wordExposure = new Dictionary<string, int>(); // Tracks exposure count
    private HashSet<string> learnedWords = new HashSet<string>();
    private Dictionary<string, float> wordCooldown = new Dictionary<string, float>(); // Prevents rapid learning
    private float learnCooldown = 5f;  // 5-second cooldown

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
        List<string> translatedWords = new List<string>();

        foreach (string word in words)
        {
            if (wordDictionary.ContainsKey(word))
            {
                if (learnedWords.Contains(word))
                {
                    translatedWords.Add(wordDictionary[word]); // Fully translated
                }
                else
                {
                    translatedWords.Add(RevealPartialWord(word)); // Partial translation
                }
            }
            else
            {
                translatedWords.Add(word); // Unknown words remain as they are
            }
        }

        return ConstructGrammaticallyCorrectSentence(translatedWords);
    }

    /// <summary>
    /// Ensures translated sentences maintain proper grammar.
    /// </summary>
    private string ConstructGrammaticallyCorrectSentence(List<string> words)
    {
        if (words.Count == 0) return "";

        // Capitalize first letter
        words[0] = char.ToUpper(words[0][0]) + words[0].Substring(1);

        // Join words into a sentence
        return string.Join(" ", words) + ".";
    }

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
        if (!wordDictionary.ContainsKey(word) || learnedWords.Contains(word)) return;

        if (wordCooldown.ContainsKey(word) && Time.time - wordCooldown[word] < learnCooldown)
            return; // Prevents spamming word learning

        learnedWords.Add(word);
        wordCooldown[word] = Time.time;
        wordExposure.Remove(word); // Stop tracking exposure

        WordLearned?.Invoke(word, wordDictionary[word]); // Notify UI
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
