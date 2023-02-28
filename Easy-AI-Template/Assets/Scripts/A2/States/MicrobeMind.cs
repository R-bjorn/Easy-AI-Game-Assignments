using System.Collections;
using System.Collections.Generic;
using EasyAI;
using UnityEngine;

namespace A2.States
{
    /// <summary>
    /// The global state which microbes are always in.
    /// </summary>
    [CreateAssetMenu(menuName = "A2/States/Microbe Mind", fileName = "Microbe Mind")]
    public class MicrobeMind : State
    {
        public override void Enter(Agent agent)
        {
            // Initial roaming state
            if (agent is null)
                return;
            agent.SetState<MicrobeRoamingState>();
        }

        public override void Execute(Agent agent)
        {
            return;
        }
    }
}