using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private Vector3 originalSpawnPoint;
    private Rigidbody2D rb;

    void Start()
    {
        originalSpawnPoint = transform.position;
        rb = GetComponent<Rigidbody2D>();

        // Attempt to load the checkpoint from SaveManager
        Vector3 checkpointPos;
        if (SaveManager.LoadCheckpoint(out checkpointPos))
        {
            transform.position = checkpointPos;
        }
        else
        {
            transform.position = originalSpawnPoint;
        }
    }

    public void LoadCheckpoint()
    {
        Vector3 checkpointPos;
        if (SaveManager.LoadCheckpoint(out checkpointPos))
        {
            transform.position = checkpointPos;
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero; // Reset velocity to avoid physics issues
            }
        }
        else
        {
            transform.position = originalSpawnPoint;
        }
    }

    public void Respawn()
    {
        LoadCheckpoint();
    }
}
