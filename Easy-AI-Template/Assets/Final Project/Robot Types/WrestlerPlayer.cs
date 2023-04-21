using System;
using System.Collections.Generic;
using Final_Project.Robot_Scripts;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Final_Project.Robot_Types
{
    public class WrestlerPlayer : NetworkBehaviour
    {
        // [SerializeField] private float movementSpeed = 20f;

        // private GameObject cameraFollow;

        private Transform _spawnedLocations;
        private List<Transform> _availableLocations;
        private int _index;

        private void Awake()
        {
            _spawnedLocations = GameObject.FindGameObjectWithTag("SpawnedLocation").transform;
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (_availableLocations == null || _availableLocations.Count == 0)
            {
                // If all locations are used, reset the availableLocations list
                _availableLocations = new List<Transform>();
                foreach (Transform child in _spawnedLocations)
                {
                    _availableLocations.Add(child);
                }
            }
        }
        private void Start()
        {
            if (IsClient && IsOwner)
            {
                // _index = Random.Range(0, _availableLocations.Count - 1);
                // transform.position = _availableLocations[_index].position;
                //
                // // Remove the selected location from the availableLocations list
                // _availableLocations.RemoveAt(_index);
            
            
                PlayerFollowCamera.Instance.FollowPlayer(transform);                
            }
        }

        private void Update()
        {
            if (!IsOwner) return;
        }
    }
}
