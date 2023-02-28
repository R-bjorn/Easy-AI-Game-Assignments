using EasyAI;
using UnityEngine;

namespace A2.States
{
    /// <summary>
    /// State for microbes that are being hunted.
    /// </summary>
    [CreateAssetMenu(menuName = "A2/States/Microbe Hunted State", fileName = "Microbe Hunted State")]
    public class MicrobeHuntedState : State
    {
        public float pursuitDistance = 15f;
        public override void Enter(Agent agent)
        {
            agent.Log("I'm being hunted!");
        }

        public override void Execute(Agent agent)
        {
            if (agent is null)
                return;
            agent.Log("I should be running away but I don't know how to yet!");
            if (agent is not Microbe microbe)
                return;

            if (microbe.Hunter == null) return;
            var hunter = microbe.Hunter;
            // If this microbe escape from hunter by moving further than pursuitDistance,
            // hunter should stop hunting this microbe and both microbe should move to roaming state.
            if (!(Vector3.Distance(microbe.transform.position, hunter.transform.position) >=
                  pursuitDistance)) return;
            hunter.RemoveTargetMicrobe();
            hunter.SetState<MicrobeRoamingState>();
            microbe.SetState<MicrobeRoamingState>();
        }
        
        public override void Exit(Agent agent)
        {
            agent.Log("No longer being hunted.");
        }
    }
}