using UnityEngine;
using System.Collections;
using TMPro;

public class LanguageLearnedUI : MonoBehaviour
{
    public static LanguageLearnedUI Instance;

    public GameObject popupPanel;
    public TextMeshProUGUI popupText;
    public float displayDuration = 2f;

    private void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Hide the popup when the game starts
        if (popupPanel != null)
            popupPanel.SetActive(false);
    }

    public void Show(string message)
    {
        popupText.text = message;
        popupPanel.SetActive(true);
        CancelInvoke();
        Invoke(nameof(Hide), displayDuration);
    }

    public void Hide()
    {
        popupPanel.SetActive(false);
    }
    public void ShowPopup(string language)
    {
        if (popupText != null)
            popupText.text = $"You learned the {language} language!";

        if (popupPanel != null)
            popupPanel.SetActive(true);

        // Optional: auto-hide after 2 seconds
        StartCoroutine(HidePopupAfterDelay(2f));
    }

    private IEnumerator HidePopupAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (popupPanel != null)
            popupPanel.SetActive(false);
    }

}
