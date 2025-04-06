using UnityEngine;

public class LearnableObject : MonoBehaviour, IInteractable
{
    public string wordToLearn = "hello";
    public bool isReusable = true;
    private bool learned = false;

    public void Interact()
    {
        if (!learned || isReusable)
        {
            LanguageManager.Instance.LearnWord(wordToLearn);
            Debug.Log("You learned the word: " + wordToLearn);

            if (!isReusable)
            {
                learned = true;
                Destroy(gameObject);
            }
        }
    }
}
