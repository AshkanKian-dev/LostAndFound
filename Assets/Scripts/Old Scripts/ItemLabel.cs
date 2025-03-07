using UnityEngine;

public class ItemLabel : MonoBehaviour
{
    public string wordToLearn; // The word that will be revealed when the player finds this item

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (LanguageManager.Instance != null)
            {
                LanguageManager.Instance.LearnWord(wordToLearn);
                Debug.Log("You learned the word: " + wordToLearn);
            }

            Destroy(gameObject); // Remove the item after learning
        }
    }
}
