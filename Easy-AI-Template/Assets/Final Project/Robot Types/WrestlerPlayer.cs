using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Final_Project.Robot_Types
{
    public class WrestlerPlayer : NetworkBehaviour
    {
        [SerializeField] private float movementSpeed = 20f;

        private Transform spawnedLocations;
        
        private List<Transform> _availableLocations;
        private int index;

        private void Awake()
        {
            spawnedLocations = GameObject.FindGameObjectWithTag("SpawnedLocation").transform;
            Debug.Log("Getting spawned location" + spawnedLocations.name);
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (_availableLocations == null || _availableLocations.Count == 0)
            {
                // If all locations are used, reset the availableLocations list
                _availableLocations = new List<Transform>();
                foreach (Transform child in spawnedLocations)
                {
                    _availableLocations.Add(child);
                }
            }

            index = Random.Range(0, _availableLocations.Count - 1);
            Debug.Log("Adding player at spawned location" + _availableLocations[index].position);
            
            transform.position = _availableLocations[index].position;
            Debug.Log("Player transform : " + transform.position);
            
            // Remove the selected location from the availableLocations list
            _availableLocations.RemoveAt(index);
        }

        private void Start()
        {
            transform.position = _availableLocations[index].position;
            Debug.Log("Player transform : " + transform.position);
        }

        private void Update()
        {
            if (!IsOwner) return;
        }
    }
}
