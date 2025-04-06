using TMPro;
using UnityEngine;

public class DialogueUIManager : MonoBehaviour
{
    public static DialogueUIManager Instance;

    public GameObject dialogueBox;
    public TextMeshProUGUI dialogueText;
    public bool IsDialogueActive()
    {
        return dialogueBox.activeSelf;
    }


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (dialogueBox == null)
            Debug.LogError("DialogueUIManager: dialogueBox is not assigned in the Inspector!");
        if (dialogueText == null)
            Debug.LogError("DialogueUIManager: dialogueText is not assigned in the Inspector!");

        dialogueBox.SetActive(false); // Hide by default
    }

    public void ShowDialogue(string sentence)
    {
        if (dialogueBox != null && dialogueText != null)
        {
            dialogueBox.SetActive(true);
            dialogueText.text = sentence;
        }
    }

    public void HideDialogue()
    {
        if (dialogueBox != null)
        {
            dialogueBox.SetActive(false);
        }
    }
}
