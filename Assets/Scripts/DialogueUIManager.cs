using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUIManager : MonoBehaviour
{
    public static DialogueUIManager Instance;

    [Header("UI References")]
    public GameObject dialogueBox;
    public TextMeshProUGUI dialogueText;
    public Image portraitImage;           // drag in your PortraitImage here

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            // Destroy only this extra component, not the entire UI GameObject
            Destroy(this);
            return;
        }

        if (dialogueBox == null)
            Debug.LogError("DialogueUIManager: dialogueBox is not assigned in the Inspector!");
        if (dialogueText == null)
            Debug.LogError("DialogueUIManager: dialogueText is not assigned in the Inspector!");
        if (portraitImage == null)
            Debug.LogError("DialogueUIManager: portraitImage is not assigned in the Inspector!");

        if (dialogueBox != null)
            dialogueBox.SetActive(false);

        if (portraitImage != null)
            portraitImage.gameObject.SetActive(false);
    }

    /// <summary>
    /// Shows dialogue text and optional portrait image.
    /// Safely does nothing if references have been destroyed.
    /// </summary>
    public void ShowDialogue(string sentence, Sprite portrait = null)
    {
        if (dialogueBox == null || dialogueText == null)
            return;

        dialogueText.text = sentence;
        dialogueBox.SetActive(true);

        if (portraitImage != null)
        {
            if (portrait != null)
            {
                portraitImage.sprite = portrait;
                portraitImage.gameObject.SetActive(true);
            }
            else
            {
                portraitImage.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Hides dialogue box and portrait, if they still exist.
    /// </summary>
    public void HideDialogue()
    {
        if (dialogueBox != null)
            dialogueBox.SetActive(false);
        if (portraitImage != null)
            portraitImage.gameObject.SetActive(false);
    }

    /// <summary>
    /// Returns true only if the dialogue box still exists and is active.
    /// </summary>
    public bool IsDialogueActive()
    {
        return dialogueBox != null && dialogueBox.activeSelf;
    }
}
