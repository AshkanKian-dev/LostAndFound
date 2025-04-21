using System;
using TMPro;
using UnityEngine;

public class LanguageLearnedUI : MonoBehaviour
{
    public static LanguageLearnedUI Instance;

    [Header("UI References")]
    public GameObject popupPanel;       // The parent panel to show/hide
    public TextMeshProUGUI popupText;   // The text component to update

    private bool waitForInput;          // Should we wait for F?
    private Action onComplete;          // Optional callback when done
    private bool activePopup;           // Is a popup currently shown?

    void Awake()
    {
        // Singleton setup
        if (Instance == null) Instance = this;
        else { Destroy(this); return; }

        // Hide at start
        if (popupPanel != null)
            popupPanel.SetActive(false);
    }

    void Update()
    {
        // If we're waiting for F, dismiss on key press
        if (activePopup && waitForInput && Input.GetKeyDown(KeyCode.F))
            EndPopup();
    }

    /// <summary>
    /// Show a custom message.
    /// </summary>
    /// <param name="message">Text to display.</param>
    /// <param name="requireInput">If true, waits for F to close; otherwise auto‐hides.</param>
    /// <param name="autoHideSec">Seconds before auto‐hide (if not waiting for input).</param>
    /// <param name="onDone">Callback when the popup closes.</param>
    public void Show(string message, bool requireInput = false, float autoHideSec = 2f, Action onDone = null)
    {
        if (popupPanel == null || popupText == null) return;

        popupText.text = message;
        popupPanel.SetActive(true);

        activePopup = true;
        waitForInput = requireInput;
        onComplete = onDone;

        // Cancel any pending hide, then schedule if needed
        CancelInvoke(nameof(EndPopup));
        if (!requireInput)
            Invoke(nameof(EndPopup), autoHideSec);
    }

    /// <summary>
    /// Legacy-style helper: “You learned the X language!”  
    /// (auto‐hides after 2 seconds)
    /// </summary>
    public void ShowPopup(string language)
    {
        Show($"You learned the {language} language!", requireInput: false, autoHideSec: 2f);
    }

    /// <summary>
    /// Immediately hides the popup and fires the callback.
    /// </summary>
    private void EndPopup()
    {
        if (popupPanel != null)
            popupPanel.SetActive(false);

        activePopup = false;
        onComplete?.Invoke();
    }
}
