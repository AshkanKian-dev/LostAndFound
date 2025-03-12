using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered DeathZone! Respawning...");
            other.GetComponent<PlayerRespawn>().Respawn();
        }
    }
}
