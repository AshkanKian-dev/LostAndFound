using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Debug.Log("Player collided with DeathZone! Game over triggered.");

            // Find the GameOverManager in the scene and show the game over screen
            GameOverManager gameOverManager = Object.FindAnyObjectByType<GameOverManager>();
            if (gameOverManager != null)
            {
                gameOverManager.ShowGameOverScreen();
            }

            // Optionally, disable the player to prevent further input
            collision.gameObject.SetActive(false);
        }
    }
}
