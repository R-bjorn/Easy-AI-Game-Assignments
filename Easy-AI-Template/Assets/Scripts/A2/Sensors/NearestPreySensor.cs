using System.Linq;
using UnityEngine;

namespace A2.Sensors
{
    /// <summary>
    /// Sensor to sense the nearest prey of the microbe.
    /// </summary>
    [DisallowMultipleComponent]
    public class NearestPreySensor : EasyAI.Sensor
    {
        /// <summary>
        /// Sense the nearest prey of the microbe.
        /// </summary>
        /// <returns>The nearest prey of the microbe or null if none is found.</returns>
        public override object Sense()
        {
            if (Agent is not Microbe microbe)
            {
                return null;
            }
            
            Microbe[] microbes = MicrobeManager.Microbes.Where(m =>  m != microbe && Vector3.Distance(microbe.transform.position, m.transform.position) < microbe.DetectionRange).ToArray();
            if (microbes.Length == 0)
            {
                return null;
            }
            
            // Microbes can eat all types of microbes that they cannot mate with. See readme for a food/mating table.
            microbes = microbe.MicrobeType switch
            {
                MicrobeManager.MicrobeType.Red => microbes.Where(m => m.MicrobeType != MicrobeManager.MicrobeType.Red && m.MicrobeType != MicrobeManager.MicrobeType.Orange && m.MicrobeType != MicrobeManager.MicrobeType.Pink).ToArray(),
                MicrobeManager.MicrobeType.Orange => microbes.Where(m => m.MicrobeType != MicrobeManager.MicrobeType.Orange && m.MicrobeType != MicrobeManager.MicrobeType.Yellow && m.MicrobeType != MicrobeManager.MicrobeType.Red).ToArray(),
                MicrobeManager.MicrobeType.Yellow => microbes.Where(m => m.MicrobeType != MicrobeManager.MicrobeType.Yellow && m.MicrobeType != MicrobeManager.MicrobeType.Green && m.MicrobeType != MicrobeManager.MicrobeType.Orange).ToArray(),
                MicrobeManager.MicrobeType.Green => microbes.Where(m => m.MicrobeType != MicrobeManager.MicrobeType.Green && m.MicrobeType != MicrobeManager.MicrobeType.Blue && m.MicrobeType != MicrobeManager.MicrobeType.Yellow).ToArray(),
                MicrobeManager.MicrobeType.Blue => microbes.Where(m => m.MicrobeType != MicrobeManager.MicrobeType.Blue && m.MicrobeType != MicrobeManager.MicrobeType.Purple && m.MicrobeType != MicrobeManager.MicrobeType.Green).ToArray(),
                MicrobeManager.MicrobeType.Purple => microbes.Where(m => m.MicrobeType != MicrobeManager.MicrobeType.Purple && m.MicrobeType != MicrobeManager.MicrobeType.Pink && m.MicrobeType != MicrobeManager.MicrobeType.Blue).ToArray(),
                _ => microbes.Where(m => m.MicrobeType is not (MicrobeManager.MicrobeType.Pink or MicrobeManager.MicrobeType.Red or MicrobeManager.MicrobeType.Purple)).ToArray()
            };

            return microbes.Length == 0 ? null : microbes.OrderBy(m => Vector3.Distance(microbe.transform.position, m.transform.position)).First();
        }
    }
}