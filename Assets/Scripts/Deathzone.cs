using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered DeathZone! Game over triggered.");

            // Find the GameOverManager in the scene and show the game over screen
            GameOverManager gameOverManager = Object.FindAnyObjectByType<GameOverManager>();
            if (gameOverManager != null)
            {
                gameOverManager.ShowGameOverScreen();
            }

            // Optionally, disable the player to prevent further input
            other.gameObject.SetActive(false);
        }
    }
}
