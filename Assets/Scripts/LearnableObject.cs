using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LearnableObject : MonoBehaviour, IInteractable
{
    [SerializeField] private List<string> learnableWords = new List<string>();
    public string wordToLearn;        // Optional: Used for fixed objects like labeled signs
    public bool isNPC = false;        // False for items, true for NPCs
    public bool isReusable = false;   // Can this be used repeatedly?
    private bool learned = false;     // Track if it's been used (for one-time objects)

    public void Interact()
    {
        if (learnableWords.Count == 0)
        {
            Debug.LogWarning($"[LearnableObject] No words assigned to {gameObject.name}.");
            return;
        }

        // Filter for unlearned words only
        List<string> unlearned = learnableWords
            .Where(word => !LanguageManager.Instance.IsWordLearned(word))
            .ToList();

        if (unlearned.Count == 0)
        {
            Debug.Log($"[LearnableObject] All words already learned from {gameObject.name}.");
            return;
        }

        // Choose one unlearned word at random
        string wordToLearn = unlearned[Random.Range(0, unlearned.Count)];

        // Teach it
        LanguageManager.Instance.LearnWord(wordToLearn);
        Debug.Log($"[LearnableObject] Interacted with {gameObject.name}, learned: {wordToLearn}");

        learned = true;
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
