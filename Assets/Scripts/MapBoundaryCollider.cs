using UnityEngine;
using UnityEngine.Tilemaps;

public class MapBoundaryCollider : MonoBehaviour
{
    // Assign your Tilemap here in the Inspector.
    public Tilemap mapTilemap;

    void Start()
    {
        if (mapTilemap == null)
        {
            Debug.LogError("MapBoundaryCollider: No Tilemap assigned!");
            return;
        }

        // Retrieve the cell bounds of the tilemap.
        BoundsInt cellBounds = mapTilemap.cellBounds;

        // Convert the cell bounds to world positions.
        Vector3 min = mapTilemap.CellToWorld(cellBounds.min);
        Vector3 max = mapTilemap.CellToWorld(cellBounds.max);

        // Calculate the center if you want to reposition the GameObject (optional).
        Vector3 center = (min + max) / 2f;
        transform.position = center;

        // Create an array of points for the EdgeCollider2D.
        Vector2[] points = new Vector2[5];
        points[0] = new Vector2(min.x, min.y);
        points[1] = new Vector2(min.x, max.y);
        points[2] = new Vector2(max.x, max.y);
        points[3] = new Vector2(max.x, min.y);
        points[4] = points[0]; // Close the loop.

        // Get the EdgeCollider2D component and set its points.
        EdgeCollider2D edgeCollider = GetComponent<EdgeCollider2D>();
        edgeCollider.points = points;
    }

}
