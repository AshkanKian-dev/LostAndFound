using UnityEngine;

public class LearnableObject : MonoBehaviour, IInteractable
{
    public string wordToLearn;    // Word that will be learned
    public NPCDialogue npcDialogue;  // For NPC-specific behavior
    public bool isNPC = false;    // False for items, true for NPCs
    public bool isReusable = false; // Whether the word source can be reused
    private bool learned = false; // Track if the word has been learned already
   

    // Called when the player presses "F" to interact
    public void Interact()
    {
        if (!learned)
        {
            // If this is an NPC, call the NPC's TeachPlayerWords() method
            if (isNPC && npcDialogue != null)
            {
                npcDialogue.TeachPlayerWords();
            }
            else
            {
                // Otherwise, teach a single word directly
                LanguageManager.Instance.LearnWord(wordToLearn);
                Debug.Log("You learned the word: " + wordToLearn);
            }

            // If not reusable and not an NPC, mark learned and destroy the object
            if (!isReusable && !isNPC)
            {
                learned = true;
                Destroy(gameObject);
            }
        }
    }

    // Detect if the player enters the interaction range
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !learned)
        {
            // Interaction logic triggers when the player presses "F"
            InteractionSystem interaction = other.GetComponent<InteractionSystem>();
            if (interaction != null)
            {
                interaction.RegisterInteractable(this);
            }
        }
    }
}
