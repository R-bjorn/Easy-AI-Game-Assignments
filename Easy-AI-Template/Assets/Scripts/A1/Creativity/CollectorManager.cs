using System;
using System.Collections.Generic;
using EasyAI;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace A1.Creativity
{
    public class CollectorManager : Manager
    {
        [Header("Prefabs")] [Tooltip("The prefab for the collecting agent that will be spawned in.")] [SerializeField]
        private GameObject collectorAgentPrefab;
        
        [Tooltip("The prefab for floor that will be spawned in.")] [SerializeField]
        private GameObject floorPrefab;
        
        [Tooltip("The prefab for the pickups that will be spawned in.")] [SerializeField]
        private List<GameObject> pickupsPrefabsList;
        
        [Tooltip("The prefab for the collectors that will be spawned in.")] [SerializeField]
        private List<GameObject> collectorsPrefabsList;
        
        [Header("Collector Parameters")]
        [Tooltip("How many seconds between every time dirt is randomly added to the floor.")]
        [Min(0)]
        [SerializeField]
        private float timeBetweenDirtGeneration = 5;
        private static CollectorManager CollectorSingleton => Singleton as CollectorManager;

        private GameObject _collectorAgent;
        private static List<GameObject> _pickupList;
        private static List<GameObject> _collectorList;
        private GameObject _floor;

        private float _elapsedTime;
        private static void GenerateFloor()
        {
            // Destroy the previous agent.
            if (CollectorSingleton._collectorAgent != null)
            {
                Destroy(CollectorSingleton._collectorAgent.gameObject);
            }
            
            // Destroy all previous floors.
            if (CollectorSingleton._floor != null)
            {
                Destroy(CollectorSingleton._floor.gameObject);
            }
            
            // Add the ground
            CollectorSingleton._floor = Instantiate(CollectorSingleton.floorPrefab, Vector3.zero, quaternion.identity);
            CollectorSingleton._floor.name = "Ground";
            
            // Add the cleaner agent.
            CollectorSingleton._collectorAgent = Instantiate(CollectorSingleton.collectorAgentPrefab, Vector3.zero, quaternion.identity);
            CollectorSingleton._collectorAgent.name = "Robot agent";
            
            // Reset elapsed Time
            CollectorSingleton._elapsedTime = 0;
        }

        private static void UpdateFloor()
        {
            CollectorSingleton._elapsedTime += Time.deltaTime;
            if (CollectorSingleton._elapsedTime < CollectorSingleton.timeBetweenDirtGeneration)
                return;
            CollectorSingleton._elapsedTime = 0;

            for(int i = 0 ; i < _pickupList.Count ; i++)
            {
                Vector3 pickupRandomPos = new Vector3(Random.Range(-4, 3), 0.5f, Random.Range(-3, 3));
                Vector3 collectorRandomPos = new Vector3(Random.Range(-5, 4), 0.1f, Random.Range(-3.5f, 4));

                int randomElement = Random.Range(0, 2);
                Instantiate(_pickupList[randomElement], pickupRandomPos, Quaternion.identity);
                Instantiate(_collectorList[randomElement], collectorRandomPos, Quaternion.identity);
            }
        }

        protected override void Start()
        {
            base.Start();
            _pickupList = pickupsPrefabsList;
            _collectorList = collectorsPrefabsList;
            GenerateFloor();
        }

        protected override void Update()
        {
            base.Update();
            UpdateFloor();
        }
    }
}
