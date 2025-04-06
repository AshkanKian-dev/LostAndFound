using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject optionsMenu;
    public AudioSource backgroundMusic;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            bool isPaused = !pauseMenu.activeSelf;
            pauseMenu.SetActive(isPaused);
            Time.timeScale = isPaused ? 0f : 1f;

            if (backgroundMusic != null)
            {
                if (isPaused)
                    backgroundMusic.Pause();
                else
                    backgroundMusic.UnPause();
            }
        }
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        Time.timeScale = 1f;

        if (backgroundMusic != null)
            backgroundMusic.UnPause();
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void OpenOptions()
    {
        if (optionsMenu != null)
            optionsMenu.SetActive(true);
    }

    public void CloseOptions()
    {
        if (optionsMenu != null)
            optionsMenu.SetActive(false);
    }
}