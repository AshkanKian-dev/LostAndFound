using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    // Assign this in the Inspector with your Game Over UI Panel
    public GameObject gameOverScreen;

    private void Start()
    {
        if (gameOverScreen != null)
            gameOverScreen.SetActive(false);
    }

    // Call this method to show the game over screen
    public void ShowGameOverScreen()
    {
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
            // Optionally pause the game
            Time.timeScale = 0f;
        }
    }

    // This method is called by the Restart button
    public void RestartGame()
    {
        Time.timeScale = 1f; // Unpause if needed
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // This method is called by the Quit button
    public void QuitGame()
    {
        Debug.Log("Quit game requested.");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
