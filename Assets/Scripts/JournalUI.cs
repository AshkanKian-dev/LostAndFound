using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JournalUI : MonoBehaviour
{
    public GameObject journalPanel; // The UI panel for the journal
    public TextMeshProUGUI dictionaryText; // Text area to display learned words

    void Start()
    {
        journalPanel.SetActive(false); // Hide journal by default
        UpdateDictionary();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J)) // Open/close journal with 'J'
        {
            journalPanel.SetActive(!journalPanel.activeSelf);
            UpdateDictionary();
        }
    }

    public void UpdateDictionary()
    {
        if (LanguageManager.Instance == null) return;

        List<string> learnedWords = LanguageManager.Instance.GetLearnedWords();
        dictionaryText.text = "Learned Words:\n";

        foreach (string word in learnedWords)
        {
            dictionaryText.text += word + " - " + LanguageManager.Instance.GetTranslation(word) + "\n";
        }
    }
}