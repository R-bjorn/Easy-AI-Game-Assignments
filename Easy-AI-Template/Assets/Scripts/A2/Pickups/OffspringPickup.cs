using EasyAI;
using UnityEngine;

namespace A2.Pickups
{
    /// <summary>
    /// Class to spawn offspring instantly without having to mate.
    /// </summary>
    [DisallowMultipleComponent]
    public class OffspringPickup : MicrobeBasePickup
    {
        [SerializeField]
        [Min(1)]
        [Tooltip("How many offspring to spawn.")]
        private int spawnCount = 3;
        
        /// <summary>
        /// The behaviour of the pickup.
        /// </summary>
        /// <param name="microbe">The microbe which picked up this pickup.</param>
        protected override void Execute(Microbe microbe)
        {
            microbe.Log("Powered up - magically spawned offspring!");
            for (int i = 0; i < spawnCount && Manager.CurrentAgents.Count < MicrobeManager.MaxMicrobes; i++)
            {
                // Treat this as mating but with the same parent passed in for both values.
                MicrobeManager.Mate(microbe, microbe);
            }
        }
    }
}