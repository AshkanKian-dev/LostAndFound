using UnityEngine;
using UnityEngine.UI;       // For RectTransform
using UnityEngine.Tilemaps; // For Tilemap

[RequireComponent(typeof(Camera))]
public class MiniMapCamera : MonoBehaviour
{
    [Header("Follow Settings")]
    [Tooltip("When zoomed in below the threshold, the camera will follow the target exactly.")]
    public Transform target;
    [Tooltip("Size threshold at which the camera locks onto target.")]
    public float followZoomThreshold = 30f;
    [Tooltip("How quickly the camera lerps to the follow position.")]
    public float followLerpSpeed = 10f;

    [Header("Zoom Settings")]
    [Tooltip("Base zoom speed per scroll unit.")]
    public float zoomSpeed = 2f;
    [Tooltip("Min orthographic size (zoom in limit).")]
    public float minZoom = 5f;
    [Tooltip("Max orthographic size (zoom out limit).")]
    public float maxZoom = 230f;
    [Tooltip("Smoothing factor for zoom transitions.")]
    public float zoomLerpSpeed = 10f;

    [Header("Map Boundaries")]
    [Tooltip("Assign your Tilemap here to auto-calculate bounds.")]
    public Tilemap mapTilemap;
    private Vector2 mapWorldMin;
    private Vector2 mapWorldMax;

    [Header("UI")]
    [Tooltip("RectTransform of the minimap container.")]
    public RectTransform minimapRect;

    // Internals
    private Camera cam;
    private float targetZoom;
    private bool zoomChanged;
    private Vector2 zoomNormalizedPoint;
    private Vector3 zoomWorldBefore;
    private float zoomZDist;

    private bool isDragging;
    private Vector2 lastMousePosition;

    void Awake()
    {
        cam = GetComponent<Camera>();
        // Clamp current zoom into range immediately
        cam.orthographicSize = targetZoom = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);

        if (mapTilemap != null)
        {
            mapTilemap.CompressBounds();
            Bounds b = mapTilemap.localBounds;
            Vector3 wMin = mapTilemap.transform.TransformPoint(b.min);
            Vector3 wMax = mapTilemap.transform.TransformPoint(b.max);
            mapWorldMin = new Vector2(Mathf.Min(wMin.x, wMax.x), Mathf.Min(wMin.y, wMax.y));
            mapWorldMax = new Vector2(Mathf.Max(wMin.x, wMax.x), Mathf.Max(wMin.y, wMax.y));
        }
    }

    void LateUpdate()
    {
        HandleZoom();
        HandleFollowOrDrag();
        ClampCameraPosition();
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f && minimapRect != null)
        {
            isDragging = false;

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                minimapRect, Input.mousePosition, null, out Vector2 lp))
            {
                zoomNormalizedPoint = new Vector2(
                    (lp.x + minimapRect.rect.width * 0.5f) / minimapRect.rect.width,
                    (lp.y + minimapRect.rect.height * 0.5f) / minimapRect.rect.height
                );

                zoomZDist = Mathf.Abs(cam.transform.position.z);
                zoomWorldBefore = cam.ViewportToWorldPoint(
                    new Vector3(zoomNormalizedPoint.x, zoomNormalizedPoint.y, zoomZDist)
                );

                float mul = scroll < 0 ? 2f : 1f;
                targetZoom = Mathf.Clamp(
                    targetZoom - scroll * zoomSpeed * mul,
                    minZoom, maxZoom
                );
                zoomChanged = true;
            }
        }

        // Smooth zoom
        float cur = cam.orthographicSize;
        float lerpSp = zoomLerpSpeed * (targetZoom > cur ? 2f : 1f);
        cam.orthographicSize = Mathf.Lerp(cur, targetZoom, Time.deltaTime * lerpSp);

        if (zoomChanged)
        {
            Vector3 worldAfter = cam.ViewportToWorldPoint(
                new Vector3(zoomNormalizedPoint.x, zoomNormalizedPoint.y, zoomZDist)
            );
            cam.transform.position += (zoomWorldBefore - worldAfter);
            zoomChanged = false;
        }
    }

    void HandleFollowOrDrag()
    {
        // 1) Follow (center‑lock) when zoomed in:
        if (cam.orthographicSize <= followZoomThreshold && target != null)
        {
            isDragging = false;
            Vector3 desired = target.position;
            desired.z = cam.transform.position.z;
            cam.transform.position = Vector3.Lerp(
                cam.transform.position,
                desired,
                Time.deltaTime * followLerpSpeed
            );
        }
        // 2) Pan when zoomed out:
        else if (minimapRect != null)
        {
            // start drag
            if (Input.GetMouseButtonDown(0) &&
                RectTransformUtility.RectangleContainsScreenPoint(minimapRect, Input.mousePosition))
            {
                isDragging = true;
                lastMousePosition = Input.mousePosition;
            }
            // end drag
            if (Input.GetMouseButtonUp(0))
                isDragging = false;

            // apply direct, immediate movement
            if (isDragging)
            {
                Vector2 delta = (Vector2)Input.mousePosition - lastMousePosition;
                lastMousePosition = Input.mousePosition;

                float conv = (cam.orthographicSize * 2f) / Screen.height;
                Vector3 worldDelta = new Vector3(delta.x * conv, delta.y * conv, 0);
                cam.transform.position -= worldDelta;
            }
        }
    }

    void ClampCameraPosition()
    {
        if (mapTilemap == null) return;

        float halfH = cam.orthographicSize;
        float halfW = halfH * cam.aspect;
        Vector3 p = cam.transform.position;
        p.x = Mathf.Clamp(p.x, mapWorldMin.x + halfW, mapWorldMax.x - halfW);
        p.y = Mathf.Clamp(p.y, mapWorldMin.y + halfH, mapWorldMax.y - halfH);
        cam.transform.position = p;
    }
}
