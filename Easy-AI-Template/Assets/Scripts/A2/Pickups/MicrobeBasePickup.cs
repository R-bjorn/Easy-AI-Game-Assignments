using System.Linq;
using EasyAI;
using UnityEngine;

namespace A2.Pickups
{
    /// <summary>
    /// Base class for pickups for microbes.
    /// </summary>
    [DisallowMultipleComponent]
    public abstract class MicrobeBasePickup : MonoBehaviour
    {
        /// <summary>
        /// The behaviour of the pickup.
        /// </summary>
        /// <param name="microbe">The microbe which picked up this pickup.</param>
        protected abstract void Execute(Microbe microbe);

        private void Update()
        {
            // Get all microbes near to this pickup.
            Microbe[] microbes = Manager.CurrentAgents.Where(a => Vector3.Distance(a.transform.position, transform.position) <= MicrobeManager.MicrobeInteractRadius).Cast<Microbe>().ToArray();
            if (microbes.Length == 0)
            {
                return;
            }
            
            // Activate this pickup for the nearest microbe.
            Microbe microbe = microbes.OrderBy(m => Vector3.Distance(m.transform.position, transform.position)).First();
            microbe.Log("Collecting pickup.");
            microbe.PlayPickupAudio();
            Execute(microbe);
            Instantiate(MicrobeManager.PickupParticlesPrefab, microbe.transform.position, Quaternion.Euler(270, 0, 0));
            Destroy(gameObject);
        }
    }
}