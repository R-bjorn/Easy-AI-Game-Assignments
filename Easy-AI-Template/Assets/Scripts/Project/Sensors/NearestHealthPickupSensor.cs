using EasyAI;
using UnityEngine;

namespace Project.Sensors
{
    /// <summary>
    /// Sensor to sense the nearest health pickup to a soldier.
    /// </summary>
    [DisallowMultipleComponent]
    public class NearestHealthPickupSensor : Sensor
    {
        /// <summary>
        /// Sense the nearest health pickup to a soldier.
        /// </summary>
        /// <returns>The nearest available health pickup or null if no pickups available.</returns>
        public override object Sense()
        {
            return SoldierManager.NearestHealthPickup(Agent);
        }
    }
}