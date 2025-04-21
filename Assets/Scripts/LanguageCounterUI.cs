using UnityEngine;
using TMPro;

public class LanguageCounterUI : MonoBehaviour
{
    [Header("How many you’ve learned / total")]
    [SerializeField] private TextMeshProUGUI counterText;

    private void Awake()
    {
        // if you attached this script directly to the TMP object, grab it here:
        if (counterText == null)
            counterText = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        // 1) draw the very first “0 / total”
        RefreshCounter();

        // 2) subscribe to the LanguageManager’s event
        LanguageManager.Instance.LanguageLearnedEvent += OnNewLanguage;
    }

    private void OnDisable()
    {
        LanguageManager.Instance.LanguageLearnedEvent -= OnNewLanguage;
    }

    private void OnNewLanguage(string langName)
    {
        RefreshCounter();
    }

    private void RefreshCounter()
    {
        int learned = LanguageManager.Instance.LearnedLanguageCount;
        int total = LanguageManager.Instance.allLanguages.Length;
        counterText.text = $"{learned} / {total}";
    }
}
