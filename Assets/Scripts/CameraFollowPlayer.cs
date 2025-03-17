using UnityEditor.Rendering.Universal;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float followThreshold = 2.0f; // Distance from the center before the camera starts following
    [SerializeField] private float smoothSpeed = 0.125f; // Speed of the camera smoothing

    private void LateUpdate()
    {
        Vector3 currentPosition = transform.position;
        float playerY = playerTransform.position.y;

        // Calculate the top and bottom bounds of the dead zone
        float topBound = currentPosition.y + followThreshold;
        float bottomBound = currentPosition.y - followThreshold;

        // Check if the player is outside the dead zone
        if (playerY > topBound)
        {
            currentPosition.y = playerY - followThreshold;
        }
        else if (playerY < bottomBound)
        {
            currentPosition.y = playerY + followThreshold;
        }

        // Smoothly interpolate the camera's position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, currentPosition, smoothSpeed);
        transform.position = new Vector3(0, smoothedPosition.y, -10);
    }
}