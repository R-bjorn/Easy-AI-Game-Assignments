using System.Linq;
using A2.States;
using EasyAI;
using UnityEngine;

namespace A2.Sensors
{
    /// <summary>
    /// Sensor to sense the nearest potential mate of the microbe.
    /// </summary>
    [DisallowMultipleComponent]
    public class NearestMateSensor : Sensor
    {
        /// <summary>
        /// Sense the nearest potential mate of the microbe.
        /// </summary>
        /// <returns>The nearest potential mate of the microbe or null if none is found.</returns>
        public override object Sense()
        {
            if (Agent is not Microbe microbe)
            {
                return null;
            }

            Microbe[] microbes = MicrobeManager.Microbes.Where(m => m != microbe && m.IsAdult && m.IsInState<MicrobeMatingState>() && Vector3.Distance(microbe.transform.position, m.transform.position) < microbe.DetectionRange).ToArray();
            if (microbes.Length == 0)
            {
                return null;
            }
            
            // Microbes can mate with a type/color one up or down from theirs in additional to their own color. See readme for a food/mating table.
            microbes = microbe.MicrobeType switch
            {
                MicrobeManager.MicrobeType.Red => microbes.Where(m => m.MicrobeType is MicrobeManager.MicrobeType.Red or MicrobeManager.MicrobeType.Orange or MicrobeManager.MicrobeType.Pink).ToArray(),
                MicrobeManager.MicrobeType.Orange => microbes.Where(m => m.MicrobeType is MicrobeManager.MicrobeType.Orange or MicrobeManager.MicrobeType.Yellow or MicrobeManager.MicrobeType.Red).ToArray(),
                MicrobeManager.MicrobeType.Yellow => microbes.Where(m => m.MicrobeType is MicrobeManager.MicrobeType.Yellow or MicrobeManager.MicrobeType.Green or MicrobeManager.MicrobeType.Orange).ToArray(),
                MicrobeManager.MicrobeType.Green => microbes.Where(m => m.MicrobeType is MicrobeManager.MicrobeType.Green or MicrobeManager.MicrobeType.Blue or MicrobeManager.MicrobeType.Yellow).ToArray(),
                MicrobeManager.MicrobeType.Blue => microbes.Where(m => m.MicrobeType is MicrobeManager.MicrobeType.Blue or MicrobeManager.MicrobeType.Purple or MicrobeManager.MicrobeType.Green).ToArray(),
                MicrobeManager.MicrobeType.Purple => microbes.Where(m => m.MicrobeType is MicrobeManager.MicrobeType.Purple or MicrobeManager.MicrobeType.Pink or MicrobeManager.MicrobeType.Blue).ToArray(),
                _ => microbes.Where(m => m.MicrobeType is MicrobeManager.MicrobeType.Pink or MicrobeManager.MicrobeType.Red or MicrobeManager.MicrobeType.Purple).ToArray()
            };
            
            return microbes.Length == 0 ? null : microbes.OrderBy(m => Vector3.Distance(microbe.transform.position, m.transform.position)).First();
        }
    }
}