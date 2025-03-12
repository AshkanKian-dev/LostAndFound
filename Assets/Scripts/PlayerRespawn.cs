using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private Vector3 originalSpawnPoint;

    void Start()
    {
        originalSpawnPoint = transform.position;

        // Always reset checkpoint data when restarting the scene
        if (!PlayerPrefs.HasKey("CheckpointTouched"))
        {
            Checkpoint.ResetCheckpoint();
        }
    }

    public void LoadCheckpoint()
    {
        if (PlayerPrefs.HasKey("CheckpointX"))
        {
            float x = PlayerPrefs.GetFloat("CheckpointX");
            float y = PlayerPrefs.GetFloat("CheckpointY");

            transform.position = new Vector3(x, y, 0);
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
