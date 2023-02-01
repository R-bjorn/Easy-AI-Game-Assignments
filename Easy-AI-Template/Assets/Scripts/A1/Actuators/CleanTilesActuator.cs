using EasyAI;
using UnityEngine;

namespace A1.Actuators
{
    /// <summary>
    /// Actuator to clean dirty tiles
    /// </summary>
    [DisallowMultipleComponent]
    public class CleanTilesActuator : Actuator
    {
        [Tooltip("How far away from the box must the agent be to pick it up.")]
        [Min(float.Epsilon)]
        [SerializeField]
        private float collectDistance = 1;

        // [Tooltip("How much time does it take to clean one dirty tile")] 
        // [Min(float.Epsilon)] [SerializeField]
        // private float cleaningTime = 2;

        public override bool Act(object agentAction)
        {
            if(agentAction is not Transform dirtyTile)
            {
                return false;
            }

            if (Vector3.Distance(Agent.transform.position, dirtyTile.position) > collectDistance)
            {
                Log("Not close enough to clean the dirty tile!");
                return false;
            }
 
            // Action to clean the dirty tile.
            dirtyTile.gameObject.GetComponent<Floor>().Clean();
            return true;
        }
    }
}
