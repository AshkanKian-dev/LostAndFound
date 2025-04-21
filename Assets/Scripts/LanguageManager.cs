using System.Collections.Generic;
using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager Instance;

    [System.Serializable]
    public struct WordEntry { public string key; public string translation; }

    [Header("Dictionary")]
    public List<WordEntry> wordList;

    [Header("Win Condition")]
    [Tooltip("List every LanguageData in your game here")]
    public LanguageData[] allLanguages;

    // internal state
    private Dictionary<string, string> dict;
    private HashSet<string> learnedWords = new HashSet<string>();
    private HashSet<string> learnedLanguages = new HashSet<string>();

    // Event fired any time a language is newly learned
    public delegate void OnLanguageLearned(string languageName);
    public event OnLanguageLearned LanguageLearnedEvent;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // build our lookup dictionary
            dict = new Dictionary<string, string>();
            foreach (var e in wordList)
                dict[e.key] = e.translation;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Call to mark a full language as learned.
    /// Will fire the pop‑up, the event, and test for win condition.
    /// </summary>
    public void LearnLanguage(string lang)
    {
        if (learnedLanguages.Add(lang))
        {
            // NO MORE popup here!

            // notify any listeners (e.g. exit NPC)
            LanguageLearnedEvent?.Invoke(lang);

            // check win condition
            if (learnedLanguages.Count >= allLanguages.Length)
                GameEndManager.Instance?.UnlockExit();
        }
    }


    /// <summary>
    /// Query whether the player knows a given language.
    /// </summary>
    public bool KnowsLanguage(string lang)
    {
        return learnedLanguages.Contains(lang);
    }

    /// <summary>
    /// Call to mark a single word as learned.
    /// Fires a small pop‑up “word = translation.”
    /// </summary>
    public void LearnWord(string word)
    {
        if (dict.ContainsKey(word) && learnedWords.Add(word))
        {
            LanguageLearnedUI.Instance?.Show($"{word} = {dict[word]}");
        }
    }

    /// <summary>
    /// Translate a sentence, replacing only the words you’ve learned.
    /// </summary>
    public string TranslateSentence(string sentence)
    {
        var parts = sentence.Split(' ');
        for (int i = 0; i < parts.Length; i++)
        {
            if (learnedWords.Contains(parts[i]) && dict.TryGetValue(parts[i], out var t))
                parts[i] = t;
        }
        return string.Join(" ", parts);
    }

    /// <summary>
    /// For your journal UI.
    /// </summary>
    public List<string> GetLearnedWords()
    {
        return new List<string>(learnedWords);
    }

    /// <summary>
    /// For your journal UI.
    /// </summary>
    public string GetTranslation(string word)
    {
        return dict.TryGetValue(word, out var t) ? t : word;
    }

    /// <summary>
    /// How many languages have you learned so far?
    /// </summary>
    public int LearnedLanguageCount => learnedLanguages.Count;
}
