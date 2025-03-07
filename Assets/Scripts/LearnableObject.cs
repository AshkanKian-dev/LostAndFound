using UnityEngine;

public class LearnableObject : MonoBehaviour, IInteractable
{
    public string wordToLearn;  // Word that will be learned
    public NPCDialogue npcDialogue;  // For NPC-specific behavior
    public bool isNPC = false;  // False for items, true for NPCs
    public bool isReusable = false;  // Whether the word source can be reused by the player
    private bool learned = false;  // To track if the word has been learned already

    // Implement the IInteractable interface's Interact method
    public void Interact()
    {
        if (!learned)
        {
            if (isNPC && npcDialogue != null)
            {
                npcDialogue.TeachPlayerWord(wordToLearn);  // Handle NPC teaching word
            }
            else
            {
                LanguageManager.Instance.LearnWord(wordToLearn);  // Handle item or book teaching word
                Debug.Log("You learned the word: " + wordToLearn);
            }

            if (!isReusable)
            {
                learned = true;  // Prevent learning again if not reusable
                Destroy(gameObject);  // Destroy object if it's not reusable (like in ItemLabel)
            }
        }
    }

    // Detect if the player enters the interaction range
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !learned)
        {
            // Interaction logic now triggers when the player presses "F"
            InteractionSystem interaction = other.GetComponent<InteractionSystem>();
            if (interaction != null)
            {
                interaction.RegisterInteractable(this);  // Register this object for interaction
            }
        }
    }
}
