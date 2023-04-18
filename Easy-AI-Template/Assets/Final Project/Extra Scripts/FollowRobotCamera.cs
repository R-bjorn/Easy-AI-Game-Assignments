using UnityEngine;

namespace Final_Project.Extra_Scripts
{
    public class FollowRobotCamera : MonoBehaviour
    {
        public Transform playerTransform; // Reference to the player's transform
        public Vector3 offset; // Offset from the player's position
        public float angle; // Angle around the player to look at (in degrees)
        public float smoothing = 5f; // How quickly the camera should move towards its target position

        private void LateUpdate()
        {
            // Calculate the target position for the camera based on the player's position and the offset and angle values
            Vector3 targetPosition = playerTransform.position + Quaternion.Euler(0f, angle, 0f) * offset;

            // Move the camera towards the target position using smoothing
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing * Time.deltaTime);

            // Rotate the camera to look at the player
            transform.LookAt(playerTransform.position);
        }
    }
}