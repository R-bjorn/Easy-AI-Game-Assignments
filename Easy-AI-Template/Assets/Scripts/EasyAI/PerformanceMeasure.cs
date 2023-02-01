using EasyAI.Utility;
using UnityEngine;

namespace EasyAI
{
    /// <summary>
    /// Base class for implementing a formula to calculate the performance of an agent.
    /// </summary>
    [DisallowMultipleComponent]
    public abstract class PerformanceMeasure : IntelligenceComponent
    {
        /// <summary>
        /// Implement to calculate the performance.
        /// </summary>
        /// <returns>The calculated performance.</returns>
        public abstract float CalculatePerformance();
    }
}