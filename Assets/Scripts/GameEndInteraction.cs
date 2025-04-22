using UnityEngine;

public class GameEndInteraction : MonoBehaviour, IInteractable
{
    private bool exitUnlocked = false;

    void OnEnable()
    {
        // subscribe so we know when ALL languages are learned
        LanguageManager.Instance.LanguageLearnedEvent += OnLanguageLearned;
    }

    void OnDisable()
    {
        LanguageManager.Instance.LanguageLearnedEvent -= OnLanguageLearned;
    }

    private void OnLanguageLearned(string lang)
    {
        // check if we've now learned them all
        if (LanguageManager.Instance.LearnedLanguageCount >=
            LanguageManager.Instance.allLanguages.Length)
        {
            exitUnlocked = true;
        }
    }

    public void Interact()
    {
        if (!exitUnlocked)
        {
            DialogueUIManager.Instance.ShowDialogue(
                "I still need to find my passport."
            );
            return;
        }

        // final win‐dialogue and cutscene
        DialogueUIManager.Instance.ShowDialogue("freedom at last!");
        GameEndManager.Instance.TriggerEndingCutscene();
    }
}
