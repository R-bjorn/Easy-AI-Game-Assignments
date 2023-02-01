using EasyAI;
using UnityEngine;

namespace Project.Sensors
{
    /// <summary>
    /// Sensor to sense a random defensive position.
    /// </summary>
    [DisallowMultipleComponent]
    public class RandomDefensivePositionSensor : Sensor
    {
        /// <summary>
        /// Sense a random defensive position.
        /// </summary>
        /// <returns>A random defensive position.</returns>
        public override object Sense()
        {
            return Agent is not Soldier soldier ? null : SoldierManager.RandomStrategicPosition(soldier, true);
        }
    }
}