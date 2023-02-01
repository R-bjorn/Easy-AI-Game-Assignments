using EasyAI;
using T2.Sensors;
using UnityEngine;

namespace T2.States
{
    /// <summary>
    /// The global state which the energy demo agent is always in.
    /// </summary>
    [CreateAssetMenu(menuName = "T2/States/Energy Mind", fileName = "Energy Mind")]
    public class EnergyMind : State
    {
        public override void Execute(Agent agent)
        {
            // Get the energy component. If there is none do nothing.
            EnergyComponent energyComponent = agent.Sense<EnergySensor, EnergyComponent>();
            if (energyComponent == null)
            {
                return;
            }

            // If out of energy, go into the rest state.
            if (energyComponent.Energy <= 0)
            {
                agent.SetState<EnergyRestState>();
                return;
            }
            
            // Otherwise if energy has fully recharged, go into the move state.
            if (energyComponent.Energy >= energyComponent.MaxEnergy)
            {
                agent.SetState<EnergyMoveState>();
            }
        }
    }
}