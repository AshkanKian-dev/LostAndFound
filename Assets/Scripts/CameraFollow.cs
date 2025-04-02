using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // The player transform that the camera will follow
    public Transform target;

    // Offset between the camera and the player
    public Vector3 offset = new Vector3(0, 0, -10);

    // How quickly the camera catches up to the player
    public float smoothSpeed = 0.125f;

    void LateUpdate()
    {
        if (target != null)
        {
            // Calculate the desired position based on the player's position and the offset
            Vector3 desiredPosition = target.position + offset;
            // Smoothly interpolate between the current position and the desired position
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}

