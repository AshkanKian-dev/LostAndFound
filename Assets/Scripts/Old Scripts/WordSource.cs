using UnityEngine;

public class WordSource : MonoBehaviour
{
    public string wordToLearn; // Word this source teaches
    public bool isReusable = false; // If true, player can learn from it multiple times

    private bool learned = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !learned)
        {
            LanguageManager.Instance.LearnWord(wordToLearn);
            Debug.Log("You learned the word: " + wordToLearn);

            if (!isReusable)
            {
                learned = true; // Prevent re-learning if not reusable
            }
        }
    }
}
