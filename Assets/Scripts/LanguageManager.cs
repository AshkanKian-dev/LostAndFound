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
        else Destroy(gameObject);
    }

    /// <summary>
    /// Call to mark a full language as learned.
    /// Fires the event and only unlocks exit if you have actually
    /// defined a non‑empty allLanguages list AND met that count.
    /// </summary>
    public void LearnLanguage(string lang)
    {
        if (!learnedLanguages.Add(lang))
            return;

        // notify any listeners (e.g. exit NPC)
        LanguageLearnedEvent?.Invoke(lang);

        // check win condition only if you have a real list
        if (allLanguages != null
            && allLanguages.Length > 0
            && learnedLanguages.Count >= allLanguages.Length)
        {
            GameEndManager.Instance?.UnlockExit();
        }
    }

    public bool KnowsLanguage(string lang) => learnedLanguages.Contains(lang);

    public void LearnWord(string word)
    {
        if (dict.ContainsKey(word) && learnedWords.Add(word))
            LanguageLearnedUI.Instance?.Show($"{word} = {dict[word]}");
    }

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

    public List<string> GetLearnedWords() => new List<string>(learnedWords);
    public string GetTranslation(string word) => dict.TryGetValue(word, out var t) ? t : word;
    public int LearnedLanguageCount => learnedLanguages.Count;
}
