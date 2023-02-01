using EasyAI;
using T1.Sensors;
using UnityEngine;

namespace T1.States
{
    /// <summary>
    /// The global state which the box collector is always in.
    /// </summary>
    [CreateAssetMenu(menuName = "T1/States/Box Collector Mind", fileName = "Box Collector Mind")]
    public class BoxCollectorMind : State
    {
        public override void Execute(Agent agent)
        {
            // If already moving towards a box, no need to think of anything new so simply return.
            // As mentioned in the actuator comments, you can probably see how passing transforms around will
            // become confusing in more complex agents and you can instead wrap data into of unique classes to pass.
            if (agent.HasAction<Transform>())
            {
                return;
            }

            // Sense the nearest box.
            Transform box = agent.Sense<NearestBoxSensor, Transform>();
            
            // If there are no boxes left, do nothing.
            if (box == null)
            {
                agent.Log("Collected all boxes.");
                return;
            }
            
            // Move towards the box and try to pick it up.
            agent.Log($"Collecting {box.name} next.");
            agent.Move(box.position);
            agent.Act(box);
        }
    }
}