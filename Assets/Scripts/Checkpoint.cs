using UnityEngine;
using UnityEngine.UIElements;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SaveCheckpoint(other.transform.position);
        }
    }

    void SaveCheckpoint(Vector3 position)
    {
        PlayerPrefs.SetFloat("CheckpointX", position.x);
        PlayerPrefs.SetFloat("CheckpointY", position.y);
        PlayerPrefs.SetInt("CheckpointTouched", 1); // Mark checkpoint as reached
        PlayerPrefs.Save();
    }

    public static void ResetCheckpoint()
    {
        PlayerPrefs.DeleteKey("CheckpointX");
        PlayerPrefs.DeleteKey("CheckpointY");
        PlayerPrefs.DeleteKey("CheckpointTouched"); // Remove checkpoint flag
        PlayerPrefs.Save();
    }
}
