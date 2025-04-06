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

    private Dictionary<string, int> wordExposure = new Dictionary<string, int>();
    private HashSet<string> learnedWords = new HashSet<string>();
    private Dictionary<string, float> wordCooldown = new Dictionary<string, float>();
    private float learnCooldown = 5f;

    private HashSet<string> learnedLanguages = new HashSet<string>(); // 🔥 NEW

    public delegate void OnWordLearned(string word, string translation);
    public event OnWordLearned WordLearned;

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

    // 🔥 NEW - Learn a full language (e.g., Ancient, Highland, Temple)
    public void LearnLanguage(string languageName)
    {
        if (!learnedLanguages.Contains(languageName))
        {
            learnedLanguages.Add(languageName);
            Debug.Log("Learned new language: " + languageName);
        }
    }

    // 🔥 NEW - Check if the player knows a specific language
    public bool KnowsLanguage(string languageName)
    {
        return learnedLanguages.Contains(languageName);
    }

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
                    translatedWords.Add(wordDictionary[word]);
                }
                else
                {
                    translatedWords.Add(RevealPartialWord(word));
                }
            }
            else
            {
                translatedWords.Add(word);
            }
        }

        return ConstructGrammaticallyCorrectSentence(translatedWords);
    }

    private string ConstructGrammaticallyCorrectSentence(List<string> words)
    {
        if (words.Count == 0) return "";
        words[0] = char.ToUpper(words[0][0]) + words[0].Substring(1);
        return string.Join(" ", words) + ".";
    }

    private string RevealPartialWord(string word)
    {
        if (!wordExposure.ContainsKey(word))
            wordExposure[word] = 0;

        wordExposure[word]++;

        int revealCount = Mathf.Clamp(wordExposure[word], 1, wordDictionary[word].Length);
        char[] revealedWord = GenerateConsistentGibberish(word).ToCharArray();

        for (int i = 0; i < revealCount; i++)
        {
            revealedWord[i] = wordDictionary[word][i];
        }

        return new string(revealedWord);
    }

    private string GenerateConsistentGibberish(string word)
    {
        const string letters = "abcdefghijklmnopqrstuvwxyz";
        char[] gibberish = new char[word.Length];

        for (int i = 0; i < word.Length; i++)
        {
            gibberish[i] = letters[(word[i] * 3) % letters.Length];
        }

        return new string(gibberish);
    }

    public void LearnWord(string word)
    {
        if (!wordDictionary.ContainsKey(word) || learnedWords.Contains(word)) return;

        if (wordCooldown.ContainsKey(word) && Time.time - wordCooldown[word] < learnCooldown)
            return;

        learnedWords.Add(word);
        wordCooldown[word] = Time.time;
        wordExposure.Remove(word);

        WordLearned?.Invoke(word, wordDictionary[word]);
    }

    public void LearnWordFromItem(string word)
    {
        LearnWord(word);
    }

    public List<string> GetLearnedWords()
    {
        return new List<string>(learnedWords);
    }

    public string GetTranslation(string word)
    {
        return wordDictionary.ContainsKey(word) ? wordDictionary[word] : "???";
    }
}
