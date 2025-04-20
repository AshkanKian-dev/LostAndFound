using System.Collections.Generic;
using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager Instance;

    [System.Serializable]
    public struct WordEntry { public string key; public string translation; }

    [Header("Dictionary")]
    public List<WordEntry> wordList;

    private Dictionary<string, string> dict;
    private HashSet<string> learnedWords = new HashSet<string>();
    private HashSet<string> learnedLanguages = new HashSet<string>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            dict = new Dictionary<string, string>();
            foreach (var e in wordList)
                dict[e.key] = e.translation;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Mark a full language as known
    public void LearnLanguage(string lang)
    {
        if (learnedLanguages.Add(lang))
            LanguageLearnedUI.Instance.ShowPopup(lang);
    }

    public bool KnowsLanguage(string lang) => learnedLanguages.Contains(lang);

    // Mark one word as known
    public void LearnWord(string word)
    {
        if (dict.ContainsKey(word) && learnedWords.Add(word))
            LanguageLearnedUI.Instance.Show($"{word} = {dict[word]}");
    }

    // Translate only the words you’ve learned
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

    // ─── These two let your JournalUI compile ───

    /// <summary>
    /// Returns a copy of all words the player has learned so far.
    /// </summary>
    public List<string> GetLearnedWords()
    {
        return new List<string>(learnedWords);
    }

    /// <summary>
    /// Returns the translation for a given word, or the original word if unknown.
    /// </summary>
    public string GetTranslation(string word)
    {
        return dict.TryGetValue(word, out var t) ? t : word;
    }
}
