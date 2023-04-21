using UnityEngine;

namespace Final_Project.Sensors
{
    public class NearestWeaponSensor : MonoBehaviour
    {
        // The maximum distance that the sensor can detect pickups
        public float maxDistance = 10.0f;

        // The nearest pickup object
        private GameObject _nearestPickup;

        public GameObject Sense()
        {
            // Get all the game objects with the specified pickup type
            GameObject[] pickups = GameObject.FindGameObjectsWithTag("Pickups");

            // Initialize the nearest pickup distance to a large value
            float nearestDistance = Mathf.Infinity;

            // Iterate through all the pickups to find the nearest one
            foreach (GameObject pickup in pickups)
            {
                // Calculate the distance between the pickup and the sensor
                float distance = Vector3.Distance(transform.position, pickup.transform.position);

                // Check if the pickup is within the maximum distance
                if (distance <= maxDistance)
                {
                    // Update the nearest pickup if this pickup is closer
                    if (distance < nearestDistance)
                    {
                        _nearestPickup = pickup;
                        nearestDistance = distance;
                    }
                }
            }
        
            return _nearestPickup;
        }
    }
}
