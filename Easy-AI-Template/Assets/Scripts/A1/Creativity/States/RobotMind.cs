using EasyAI;
using UnityEngine;

namespace A1.Creativity.States
{
    /// <summary>
    /// The global state which the cleaner is always in.
    /// </summary>
    [CreateAssetMenu(menuName = "A1/Creativity/Robot Mind", fileName = "Robot Mind")]
    public class RobotMind : State
    {
        public override void Execute(Agent agent)
        {
            if (agent.HasAction<Transform>())
            {
                return;
            }

            Transform obstacle = agent.Sense<FastestCompletionSensor, Transform>();

            if (obstacle == null)
            {
                agent.Log("No tasks are assigned!");
                return;
            }
            
            agent.Log($"Moving to {obstacle.name} next!");
            agent.Move(obstacle.position);
            agent.Act(obstacle);
        }
    }
}
