using System.Linq;
using EasyAI;
using UnityEngine;

namespace A1.Creativity
{
    
    /// <summary>
    /// Actuator to clean dirty tiles
    /// </summary>
    [DisallowMultipleComponent]
    public class PickupBoxActuator : Actuator
    {
        [Tooltip("How far away from the box must the agent be to pick it up.")]
        [Min(float.Epsilon)]
        [SerializeField]
        private float collectDistance = 1;
        
        
        public override bool Act(object agentAction)
        {
            if(agentAction is not Transform destinationTransform)
            {
                return false;
            }

            if (Vector3.Distance(Agent.transform.position, destinationTransform.position) > collectDistance)
            {
                Log("Not close enough to clean the dirty tile!");
                return false;
            }
 
            if (destinationTransform.gameObject.CompareTag("Pickups") && destinationTransform.gameObject.GetComponent<PickupObstacles>().isNotPickedUp)
            {
                // Debug.Log("Picking up the obstacle");
                destinationTransform.gameObject.GetComponent<PickupObstacles>().GrabObstacles();
            }
            else if(destinationTransform.gameObject.CompareTag("Collectors"))
            {
                // Debug.Log("Destroying the pickup");
                var gameAgent = GameObject.FindGameObjectWithTag("Player");
                // Debug.Log($"Getting Player {gameAgent.name}");
                if (gameAgent != null)
                {
                    Destroy(gameAgent.gameObject.GetComponentsInChildren<Transform>()
                        .First(t => t.gameObject.CompareTag("Pickups")).gameObject);
                    Destroy(destinationTransform.gameObject);
                }

            }
            
            return true;
        }
    }
}
