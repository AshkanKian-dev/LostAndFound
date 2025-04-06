using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(EdgeCollider2D))]
public class MapBoundaryCollider : MonoBehaviour
{
    public bool generateOnStart = false;

    private void Start()
    {
        if (generateOnStart)
        {
            GenerateBoundary();
        }
    }

    public void GenerateBoundary()
    {
        Tilemap tilemap = GetComponent<Tilemap>();
        if (tilemap == null)
        {
            Debug.LogError("Tilemap component not found!");
            return;
        }

        Bounds bounds = tilemap.localBounds;

        Vector2 bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        Vector2 topLeft = new Vector2(bounds.min.x, bounds.max.y);
        Vector2 topRight = new Vector2(bounds.max.x, bounds.max.y);
        Vector2 bottomRight = new Vector2(bounds.max.x, bounds.min.y);

        Vector2[] edgePoints = new Vector2[]
        {
            bottomLeft,
            topLeft,
            topRight,
            bottomRight,
            bottomLeft // loop back
        };

        EdgeCollider2D edge = GetComponent<EdgeCollider2D>();
        edge.points = edgePoints;
        edge.edgeRadius = 0f;
        edge.isTrigger = false;

        Debug.Log("✅ Boundary generated manually.");
    }
}
