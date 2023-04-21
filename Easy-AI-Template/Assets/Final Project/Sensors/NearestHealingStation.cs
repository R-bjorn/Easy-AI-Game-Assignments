using UnityEngine;

namespace Final_Project.Sensors
{
    public class NearestHealingStation : MonoBehaviour
    {
        // The maximum distance that the sensor can detect pickups
        public float maxDistance = 10.0f;

        // The nearest pickup object
        private GameObject _nearestStation;

        public GameObject Sense()
        {
            GameObject[] stations = GameObject.FindGameObjectsWithTag("Healing Station");
            
            // Initialize the nearest pickup distance to a large value
            float nearestDistance = Mathf.Infinity;
            
            // Iterate through all the pickups to find the nearest one
            foreach (GameObject station in stations)
            {
                // Calculate the distance between the pickup and the sensor
                float distance = Vector3.Distance(transform.position, station.transform.position);

                // Check if the pickup is within the maximum distance
                if (distance <= maxDistance)
                {
                    // Update the nearest pickup if this pickup is closer
                    if (distance < nearestDistance)
                    {
                        _nearestStation = station;
                        nearestDistance = distance;
                    }
                }
            }

            return _nearestStation;
        }
    }
}