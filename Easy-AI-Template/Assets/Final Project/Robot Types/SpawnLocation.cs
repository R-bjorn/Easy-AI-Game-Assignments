using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Final_Project.Robot_Types
{
    public class SpawnLocation : NetworkBehaviour
    {
        // [SerializeField] private Transform spawnedLocations;
        
        private List<Transform> _availableLocations;

        public override void OnNetworkSpawn()
        {
            if (_availableLocations == null || _availableLocations.Count == 0)
            {
                // If all locations are used, reset the availableLocations list
                _availableLocations = new List<Transform>();
                foreach (Transform child in transform)
                {
                    _availableLocations.Add(child);
                }
            }

            var index = Random.Range(0, _availableLocations.Count);
            transform.position = _availableLocations[index].position; 
            
            // Remove the selected location from the availableLocations list
            _availableLocations.RemoveAt(index);
        }
    }
}
