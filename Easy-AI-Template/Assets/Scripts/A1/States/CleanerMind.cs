using A1.Sensors;
using EasyAI;
using UnityEngine;

namespace A1.States
{
    /// <summary>
    /// The global state which the cleaner is always in.
    /// </summary>
    [CreateAssetMenu(menuName = "A1/States/Cleaner Mind", fileName = "Cleaner Mind")]
    public class CleanerMind : State
    {
        public override void Execute(Agent agent)
        {
            // TODO - Assignment 1 - Complete the mind of this agent along with any sensors and actuators you need.
            if (agent.HasAction<Transform>())
            {
                return;    
            }

            Transform tile = agent.Sense<NearestTileSensor, Transform>();

            if (tile == null)
            {
                agent.Log("No dirty tiles at the moment");
                return;
            }
            
            agent.Log($"Cleaning {tile.name} next!");
            agent.Move(tile.position);
            agent.Act(tile);
        }
    }
}