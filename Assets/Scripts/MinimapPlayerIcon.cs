using UnityEngine;
using UnityEngine.UI;

public class MinimapPlayerIcon : MonoBehaviour
{
    // The RectTransform of the mini map RawImage in your UI.
    public RectTransform minimapRect;
    // The player's transform.
    public Transform player;
    // The world space boundaries of your map that the mini map camera covers.
    public Vector2 mapWorldMin = new Vector2(-50, -50);
    public Vector2 mapWorldMax = new Vector2(50, 50);
    // Smoothing speed for the icon movement.
    public float smoothingSpeed = 10f;

    private RectTransform iconRect;

    void Awake()
    {
        iconRect = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (player == null || minimapRect == null) return;

        // Get the player's normalized position within the map boundaries (0 to 1).
        float normalizedX = Mathf.InverseLerp(mapWorldMin.x, mapWorldMax.x, player.position.x);
        float normalizedY = Mathf.InverseLerp(mapWorldMin.y, mapWorldMax.y, player.position.y);

        // Convert normalized values to local position in the minimap RectTransform.
        Vector2 minimapSize = minimapRect.rect.size;
        float posX = (normalizedX - 0.5f) * minimapSize.x;
        float posY = (normalizedY - 0.5f) * minimapSize.y;

        // Calculate the target position for the icon.
        Vector2 targetPos = new Vector2(posX, posY);
        // Smoothly interpolate the icon's position toward the target position.
        iconRect.anchoredPosition = Vector2.Lerp(iconRect.anchoredPosition, targetPos, Time.deltaTime * smoothingSpeed);
    }
}
