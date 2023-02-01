using EasyAI;
using UnityEngine;

namespace Project.Sensors
{
    /// <summary>
    /// Sensor to sense a random offensive position.
    /// </summary>
    [DisallowMultipleComponent]
    public class RandomOffensivePositionSensor : Sensor
    {
        /// <summary>
        /// Sense a random offensive position.
        /// </summary>
        /// <returns>A random offensive position.</returns>
        public override object Sense()
        {
            return Agent is not Soldier soldier ? null : SoldierManager.RandomStrategicPosition(soldier, false);
        }
    }
}