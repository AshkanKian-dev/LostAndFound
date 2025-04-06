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
            LanguageManager.Instance.LearnLanguage(languageToLearn.languageName);
            Debug.Log($"{gameObject.name} taught you the language: {languageToLearn.languageName}");

            if (!string.IsNullOrEmpty(WordToLearn))
                LanguageManager.Instance.LearnWord(WordToLearn);

            if (LanguageLearnedUI.Instance != null)
                LanguageLearnedUI.Instance.ShowPopup(languageToLearn.languageName);

            hasBeenLearned = true;
        }
        else
        {
            Debug.LogWarning("LanguageManager or LanguageData not set!");
        }
    }

    public string GetInteractText() => "Press F to examine";
}
