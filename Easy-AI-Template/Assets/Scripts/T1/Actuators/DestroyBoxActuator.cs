using EasyAI;
using UnityEngine;

namespace T1.Actuators
{
    /// <summary>
    /// Actuator to collect a box.
    /// </summary>
    [DisallowMultipleComponent]
    public class DestroyBoxActuator : Actuator
    {
        [Tooltip("How far away from the box must the agent be to pick it up.")]
        [Min(float.Epsilon)]
        [SerializeField]
        private float collectDistance = 1;
        
        /// <summary>
        /// Collect a box.
        /// </summary>
        /// <param name="agentAction">The action to perform.</param>
        /// <returns>True if the box was collect, false otherwise.</returns>
        public override bool Act(object agentAction)
        {
            // Cast the action into a transform. If it is anything else this actuator cannot use it so return false.
            // Passing a general transform may become hard in a multi-actuator setup.
            // In such cases making a unique class to wrap said data like transforms for each respective actuator is recommended.
            if (agentAction is not Transform box)
            {
                return false;
            }

            // Return false if not close enough to pickup the box.
            if (Vector3.Distance(Agent.transform.position, box.position) > collectDistance)
            {
                Log("Not close enough to pick up the box.");
                return false;
            }
            
            // Pickup (destroy) the box and return true indicating the action has been completed.
            Log("Picked up the box.");
            Destroy(box.gameObject);
            return true; 
        }
    }
}