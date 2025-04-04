using UnityEngine;

public class LearnableObject : MonoBehaviour, IInteractable
{
    public string wordToLearn;        // Optional: Used for fixed objects like labeled signs
    public NPCDialogue npcDialogue;  // For NPC-specific behavior
    public bool isNPC = false;        // False for items, true for NPCs
    public bool isReusable = false;   // Can this be used repeatedly?
    private bool learned = false;     // Track if it's been used (for one-time objects)

    public void Interact()
    {
        if (!learned || isReusable)
        {
            bool success = false;

            // NPC logic
            if (isNPC && npcDialogue != null)
            {
                npcDialogue.TeachPlayerWords();
                success = true;
            }
            // Object logic
            else
            {
                success = LanguageManager.Instance.LearnRandomWord();
            }

            // Destroy or mark learned only if it's not reusable and learning succeeded
            if (!isReusable && !isNPC && success)
            {
                learned = true;
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && (!learned || isReusable))
        {
            InteractionSystem interaction = other.GetComponent<InteractionSystem>();
            if (interaction != null)
            {
                interaction.RegisterInteractable(this);
            }
        }
    }
}
