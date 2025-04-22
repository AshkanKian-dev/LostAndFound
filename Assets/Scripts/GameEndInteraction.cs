using UnityEngine;

public class GameEndInteraction : MonoBehaviour, IInteractable
{
    private bool _exitUnlocked = false;

    void OnEnable()
    {
        if (LanguageManager.Instance != null)
            LanguageManager.Instance.LanguageLearnedEvent += OnLanguageLearned;
    }

    void OnDisable()
    {
        if (LanguageManager.Instance != null)
            LanguageManager.Instance.LanguageLearnedEvent -= OnLanguageLearned;
    }

    private void OnLanguageLearned(string lang)
    {
        var lm = LanguageManager.Instance;
        bool hasWinList = lm.allLanguages != null && lm.allLanguages.Length > 0;

        if (hasWinList && lm.LearnedLanguageCount >= lm.allLanguages.Length)
            _exitUnlocked = true;
    }

    public void Interact()
    {
        var lm = LanguageManager.Instance;
        bool hasWinList = lm != null
                          && lm.allLanguages != null
                          && lm.allLanguages.Length > 0;

        if (!hasWinList)
        {
            // no win condition defined → go immediately
            GameEndManager.Instance.TriggerEndingCutscene();
            return;
        }

        if (!_exitUnlocked)
        {
            int left = lm.allLanguages.Length - lm.LearnedLanguageCount;
            DialogueUIManager.Instance.ShowDialogue(
                $"You still need to learn {left} language{(left != 1 ? "s." : ".")}"
            );
            return;
        }

        // all done → trigger final cutscene
        GameEndManager.Instance.TriggerEndingCutscene();
    }
}
