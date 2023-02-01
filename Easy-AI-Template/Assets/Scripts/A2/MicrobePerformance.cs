using EasyAI;
using UnityEngine;

namespace A2
{
    /// <summary>
    /// Determine what microbe is the best my age and how many offspring it has had.
    /// </summary>
    [DisallowMultipleComponent]
    public class MicrobePerformance : PerformanceMeasure
    {
        /// <summary>
        /// How long in seconds the microbe has been alive.
        /// </summary>
        private float _timeAlive;
        
        /// <summary>
        /// Return how long the agent has been alive plus a score for how many offspring it has had.
        /// </summary>
        /// <returns>The score for the microbe.</returns>
        public override float CalculatePerformance() =>
            Agent is not Microbe microbe
                ? int.MinValue
                : _timeAlive * MicrobeManager.ScoreSeconds + microbe.Offspring * MicrobeManager.ScoreOffspring;

        private void Update()
        {
            _timeAlive += Time.deltaTime;
        }
    }
}