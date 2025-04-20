using UnityEngine;

public class LearnableObject : MonoBehaviour, IInteractable
{
    public string WordToLearn;
    public bool IsReusable = false;
    public LanguageData languageToLearn;

    private bool hasBeenLearned = false;

    public void Interact()
    {
        if (hasBeenLearned && !IsReusable) return;

        if (languageToLearn != null && LanguageManager.Instance != null)
        {
            // Teach the full language
            LanguageManager.Instance.LearnLanguage(languageToLearn.languageName);
            Debug.Log($"{gameObject.name} taught you the language: {languageToLearn.languageName}");

            // Teach a single word, if set
            if (!string.IsNullOrEmpty(WordToLearn))
                LanguageManager.Instance.LearnWord(WordToLearn);

            // Show the “you learned the X language!” popup
            LanguageLearnedUI.Instance?.ShowPopup(languageToLearn.languageName);

            // ——— Clear the quest indicator ———
            if (QuestManager.Instance != null)
                QuestManager.Instance.ClearQuestArea();

            hasBeenLearned = true;
        }
        else
        {
            Debug.LogWarning("LanguageManager or LanguageData not set!");
        }
    }

    public string GetInteractText() => "Press F to examine";
}
