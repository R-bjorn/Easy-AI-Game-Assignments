using EasyAI.Utility;

namespace EasyAI
{
    /// <summary>
    /// Base sensor class for sensing percepts and sending them back to the agent where they will be processed by its mind.
    /// </summary>
    public abstract class Sensor : IntelligenceComponent
    {
        /// <summary>
        /// Implement what the sensor will send back to the agent.
        /// </summary>
        /// <returns>The percepts sent back to the agent.</returns>
        public abstract object Sense();
    }
}