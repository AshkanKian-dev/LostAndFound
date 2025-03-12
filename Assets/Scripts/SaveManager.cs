using System.IO;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public float checkpointX;
    public float checkpointY;
}

public static class SaveManager
{
    private static string savePath => Application.persistentDataPath + "/savegame.json";

    public static void SaveCheckpoint(Vector3 position)
    {
        SaveData data = new SaveData
        {
            checkpointX = position.x,
            checkpointY = position.y
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        Debug.Log("Game Saved at: " + savePath);
    }

    public static bool LoadCheckpoint(out Vector3 position)
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            position = new Vector3(data.checkpointX, data.checkpointY, 0);
            Debug.Log("Game Loaded!");
            return true;
        }
        else
        {
            position = Vector3.zero;
            Debug.LogWarning("No save file found.");
            return false;
        }
    }

    public static void DeleteSave()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            Debug.Log("Save file deleted.");
        }
    }
}
