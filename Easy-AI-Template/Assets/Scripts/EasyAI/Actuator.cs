using EasyAI.Utility;

namespace EasyAI
{
    /// <summary>
    /// Base actuator class for performing actions requested by the agent.
    /// </summary>
    public abstract class Actuator : IntelligenceComponent
    {
        /// <summary>
        /// Implement how the actuator will act to any given action, if at all.
        /// </summary>
        /// <param name="agentAction">The action the agent wants to perform.</param>
        /// <returns>True if the action has been completed, false otherwise.</returns>
        public abstract bool Act(object agentAction);
    }
}