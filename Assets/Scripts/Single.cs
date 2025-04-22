using UnityEngine;
using UnityEngine.EventSystems;

[DisallowMultipleComponent]
public class SingletonEventSystem : MonoBehaviour
{
    void Awake()
    {
        // use the new API and skip sorting overhead
        var all = Object.FindObjectsByType<EventSystem>(FindObjectsSortMode.None);
        if (all.Length > 1)
            Destroy(gameObject);
        else
            DontDestroyOnLoad(gameObject);
    }
}
