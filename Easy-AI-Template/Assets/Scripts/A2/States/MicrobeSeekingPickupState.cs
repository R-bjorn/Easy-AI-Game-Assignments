using A2.Pickups;
using A2.Sensors;
using EasyAI;
using UnityEngine;

namespace A2.States
{
    /// <summary>
    /// State for microbes that are seeking a pickup.
    /// </summary>
    [CreateAssetMenu(menuName = "A2/States/Microbe Seeking Pickup State", fileName = "Microbe Seeking Pickup State")]
    public class MicrobeSeekingPickupState : State
    {
        public override void Enter(Agent agent)
        {
            agent.Log("Moving to nearest pickup");
        }
        
        public override void Execute(Agent agent)
        {
            if (agent is null)
                return;
            if(agent.HasAction<MicrobeBasePickup>())
                return;
            
            // Getting agent as microbe variable
            Microbe microbe = agent as Microbe;
            if (microbe is null)
                return;
            
            // Getting the nearest pickup using microbe sensor
            MicrobeBasePickup pickup = microbe.Sense<NearestPickupSensor, MicrobeBasePickup>();
            if (pickup == null)
            {
                microbe.Log("No pickups to eat");
                return;
            }

            // Moving towards pickup
            microbe.Move(pickup.transform.position);
            microbe.Act(pickup);
            
            if (microbe.Pickup == null)
            {
                microbe.ClearActions();
                microbe.SetState<MicrobeRoamingState>();
            }
        }
        
        public override void Exit(Agent agent)
        {
            agent.Log("At the pickup location.");
        }
    }
}