using System.Linq;
using A2.Pickups;
using EasyAI;
using UnityEngine;

namespace A2.Sensors
{
    /// <summary>
    /// Sensor to sense the nearest pickup of the microbe.
    /// </summary>
    [DisallowMultipleComponent]
    public class NearestPickupSensor : Sensor
    {
        /// <summary>
        /// Sense the nearest pickup of the microbe.
        /// </summary>
        /// <returns>The nearest pickup of the microbe or null if none is found.</returns>
        public override object Sense()
        {
            MicrobeBasePickup[] pickups = FindObjectsOfType<MicrobeBasePickup>();
            return pickups.Length == 0 ? null : pickups.OrderBy(p => Vector3.Distance(Agent.transform.position, p.transform.position)).FirstOrDefault();
        }
    }
}