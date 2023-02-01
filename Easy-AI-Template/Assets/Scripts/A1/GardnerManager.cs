using System.Collections.Generic;
using EasyAI;
using UnityEngine;

namespace A1
{
    
    /// <summary>
    /// Extension of AgentManager to handle Plant management.
    /// </summary>
    [DisallowMultipleComponent]
    public class GardnerManager : Manager
    {

        public static List<Plant> Plants => GardnerSingleton._plants;
        
        private static GardnerManager GardnerSingleton => Singleton as GardnerManager;

        private readonly List<Plant> _plants = new();

        private GameObject gardnerAgentPrefab;
        private static void SetPlants()
        {
            
        }

        private static void UpdatePlantStatus()
        {
            
        }
    
        // Start is called before the first frame update
        void Start()
        {
            base.Start();
            SetPlants();
        }

        // Update is called once per frame
        void Update()
        {
            base.Update();
            UpdatePlantStatus();
        }
    }
}
