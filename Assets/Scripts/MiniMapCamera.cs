using UnityEngine;
using UnityEngine.UI; // For RectTransform
using UnityEngine.Tilemaps; // For Tilemap

[RequireComponent(typeof(Camera))]
public class MiniMapCamera : MonoBehaviour
{
    // (Optional) Only used if you want the camera to follow the player when zoomed in.
    public Transform target;
    // The offset is used only when not locked to the character; you can leave it if desired.
    public Vector3 offset = new Vector3(0, 10, 0);

    // Zoom parameters
    public float zoomSpeed = 2f;         // Base zoom speed per scroll unit.
    public float minZoom = 5f;           // Minimum orthographic size.
    public float maxZoom = 50f;          // Maximum orthographic size.
    public float zoomLerpSpeed = 10f;    // Smoothing factor for zoom transitions.

    // Follow settings: When zoomed in below this threshold, the mini map camera will lock onto the player's position.
    public float followZoomThreshold = 30f; // When orthographic size is below 30, lock onto the player.
    public float followLerpSpeed = 10f;

    // Automatically computed map boundaries from the tilemap.
    [Header("Map Boundary Settings (Auto-calculated if tilemap is assigned)")]
    public Tilemap mapTilemap; // Assign your Tilemap here.
    private Vector2 mapWorldMin;
    private Vector2 mapWorldMax;

    // Reference to the UI RawImage's RectTransform (the mini map container)
    public RectTransform minimapRect;

    private Camera cam;
    private float targetZoom;

    // Variables to adjust the camera position during zoom so that the point under the mouse stays fixed.
    private bool zoomChanged = false;
    private Vector2 zoomNormalizedPoint;
    private Vector3 zoomWorldBefore;
    private float zoomZDist;

    // Variables for panning (manual control when zoomed out)
    private bool isDragging = false;
    private Vector2 lastMousePosition;

    void Awake()
    {
        cam = GetComponent<Camera>();
        targetZoom = cam.orthographicSize;

        // If a Tilemap is assigned, calculate map boundaries in world space.
        if (mapTilemap != null)
        {
            Vector3 localMin = mapTilemap.localBounds.min;
            Vector3 localMax = mapTilemap.localBounds.max;
            Vector3 worldMin = mapTilemap.transform.TransformPoint(localMin);
            Vector3 worldMax = mapTilemap.transform.TransformPoint(localMax);
            // Ensure proper min/max order
            mapWorldMin = new Vector2(Mathf.Min(worldMin.x, worldMax.x), Mathf.Min(worldMin.y, worldMax.y));
            mapWorldMax = new Vector2(Mathf.Max(worldMin.x, worldMax.x), Mathf.Max(worldMin.y, worldMax.y));
        }
    }

    void LateUpdate()
    {
        // --- Zoom Functionality: Zoom Centered on Mouse Pointer ---
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f && minimapRect != null)
        {
            // Disable panning during zoom
            isDragging = false;

            Vector2 localPoint;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(minimapRect, Input.mousePosition, null, out localPoint))
            {
                // Convert the local point to normalized coordinates (0 to 1)
                zoomNormalizedPoint = new Vector2(
                    (localPoint.x + minimapRect.rect.width * 0.5f) / minimapRect.rect.width,
                    (localPoint.y + minimapRect.rect.height * 0.5f) / minimapRect.rect.height
                );

                // Determine the z-distance from the camera to the map (assumed map at z=0)
                zoomZDist = Mathf.Abs(cam.transform.position.z);

                // Calculate the world position under the mouse pointer before the zoom change.
                zoomWorldBefore = cam.ViewportToWorldPoint(new Vector3(zoomNormalizedPoint.x, zoomNormalizedPoint.y, zoomZDist));

                // Use a multiplier for zooming out (when scroll is negative) to make it faster.
                float multiplier = scroll < 0 ? 2f : 1f;
                targetZoom = Mathf.Clamp(targetZoom - scroll * zoomSpeed * multiplier, minZoom, maxZoom);
                zoomChanged = true;
            }
        }

        // Smoothly interpolate the orthographic size toward the target zoom level.
        float currentZoom = cam.orthographicSize;
        float lerpSpeed = zoomLerpSpeed;
        if (targetZoom > currentZoom)
        {
            lerpSpeed *= 2f; // faster when zooming out
        }
        cam.orthographicSize = Mathf.Lerp(currentZoom, targetZoom, Time.deltaTime * lerpSpeed);

        // If zoom changed, adjust the camera position so that the world point under the mouse remains fixed.
        if (zoomChanged)
        {
            Vector3 worldAfter = cam.ViewportToWorldPoint(new Vector3(zoomNormalizedPoint.x, zoomNormalizedPoint.y, zoomZDist));
            Vector3 offsetDelta = zoomWorldBefore - worldAfter;
            cam.transform.position += offsetDelta;
            zoomChanged = false;
        }

        // --- Follow vs. Panning ---
        // When zoomed in (orthographic size is below the followZoomThreshold) and a target is assigned,
        // lock the camera directly onto the player's position.
        if (cam.orthographicSize <= followZoomThreshold && target != null)
        {
            // Lock directly on target (centered)
            Vector3 desiredPosition = target.position;
            desiredPosition.z = cam.transform.position.z; // maintain current z
            cam.transform.position = Vector3.Lerp(cam.transform.position, desiredPosition, Time.deltaTime * followLerpSpeed);
        }
        else
        {
            // Otherwise, allow manual panning via click and drag.
            if (minimapRect != null)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (RectTransformUtility.RectangleContainsScreenPoint(minimapRect, Input.mousePosition))
                    {
                        isDragging = true;
                        lastMousePosition = Input.mousePosition;
                    }
                }
                if (Input.GetMouseButtonUp(0))
                {
                    isDragging = false;
                }
                if (isDragging)
                {
                    Vector2 currentMousePos = Input.mousePosition;
                    Vector2 delta = currentMousePos - lastMousePosition;
                    lastMousePosition = currentMousePos;

                    float conversionFactor = (cam.orthographicSize * 2f) / Screen.height;
                    Vector3 worldDelta = new Vector3(delta.x * conversionFactor, delta.y * conversionFactor, 0);
                    cam.transform.position -= worldDelta;
                }
            }
        }

        // --- Clamp the Camera Position to the Map Boundaries ---
        ClampCameraPosition();
    }

    // Clamp the camera position so its view doesn't go outside the map boundaries.
    void ClampCameraPosition()
    {
        // Only clamp if boundaries were auto-calculated.
        if (mapTilemap == null)
            return;

        // Calculate the half-height and half-width of the camera view in world units.
        float halfHeight = cam.orthographicSize;
        float halfWidth = cam.orthographicSize * cam.aspect;

        Vector3 pos = cam.transform.position;
        pos.x = Mathf.Clamp(pos.x, mapWorldMin.x + halfWidth, mapWorldMax.x - halfWidth);
        pos.y = Mathf.Clamp(pos.y, mapWorldMin.y + halfHeight, mapWorldMax.y - halfHeight);
        cam.transform.position = pos;
    }
}
