using EasyAI;
using T2.Actions;
using UnityEngine;

namespace T2.Actuators
{
    /// <summary>
    /// Actuator to replenish or deplete energy.
    /// </summary>
    [DisallowMultipleComponent]
    public class EnergyActuator : Actuator
    {
        /// <summary>
        /// Replenish or deplete energy.
        /// </summary>
        /// <param name="agentAction">The action to perform.</param>
        /// <returns>Always true as this is done instantly and can never fail.</returns>
        public override bool Act(object agentAction)
        {
            switch (agentAction)
            {
                // If it is a restore energy action, restore the energy.
                case RestoreEnergyAction restoreEnergyAction:
                    restoreEnergyAction.EnergyComponent.Replenish(Agent.DeltaTime);
                    break;
                // Otherwise if it is a deplete energy action, deplete the energy.
                case DepleteEnergyAction depleteEnergyAction:
                    depleteEnergyAction.EnergyComponent.Deplete(Agent.DeltaTime);
                    break;
            }

            return true;
        }
    }
}