using EasyAI;
using UnityEngine;

namespace T2.Sensors
{
    /// <summary>
    /// Sensor to sense how much energy an agent has.
    /// </summary>
    [DisallowMultipleComponent]
    public class EnergySensor : Sensor
    {
        /// <summary>
        /// Sense how much energy an agent has.
        /// </summary>
        /// <returns>The agent's energy component or null if it doesn't have one.</returns>
        public override object Sense()
        {
            // Getting a component every time is inefficient, in real use you should cache this value.
            return Agent.GetComponent<EnergyComponent>();
        }
    }
}