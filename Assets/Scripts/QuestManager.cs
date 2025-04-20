using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    [Tooltip("Drag in your AreaIndicator prefab here")]
    public GameObject areaIndicatorPrefab;

    private GameObject currentIndicator;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    /// <summary>
    /// Spawns a ring at worldPosition. Call this when the NPC gives you the quest.
    /// </summary>
    public void ShowQuestArea(Vector3 worldPosition)
    {
        if (currentIndicator != null) Destroy(currentIndicator);
        currentIndicator = Instantiate(
            areaIndicatorPrefab,
            worldPosition,
            Quaternion.identity
        );
    }

    /// <summary>
    /// Clears the quest indicator. Call when they actually learn the language.
    /// </summary>
    public void ClearQuestArea()
    {
        if (currentIndicator != null) Destroy(currentIndicator);
    }
}
