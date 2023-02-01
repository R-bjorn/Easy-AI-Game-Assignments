using EasyAI;
using T2.Actions;
using T2.Sensors;
using UnityEngine;

namespace T2.States
{
    /// <summary>
    /// The state which the energy demo agent rests in.
    /// </summary>
    [CreateAssetMenu(menuName = "T2/States/Energy Rest State", fileName = "Energy Rest State")]
    public class EnergyRestState : State
    {
        public override void Enter(Agent agent)
        {
            agent.Log("I've got to recharge.");
        }

        public override void Execute(Agent agent)
        {
            agent.Log("Replenishing...");
            
            // Create deplete energy action.
            agent.Act(new RestoreEnergyAction(agent.Sense<EnergySensor, EnergyComponent>()));
        }

        public override void Exit(Agent agent)
        {
            agent.Log("Got all energy back.");
        }
    }
}