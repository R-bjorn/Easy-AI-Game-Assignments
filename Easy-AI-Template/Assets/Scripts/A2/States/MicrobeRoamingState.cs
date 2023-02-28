using EasyAI;
using UnityEngine;

namespace A2.States
{
    /// <summary>
    /// Roaming state for the microbe, doesn't have any actions and only logs messages.
    /// </summary>
    [CreateAssetMenu(menuName = "A2/States/Microbe Roaming State", fileName = "Microbe Roaming State")]
    public class MicrobeRoamingState : State
    {
        private bool _first = true;
        public override void Enter(Agent agent)
        {
            agent.Log("Nothing to do, start roaming");
        }

        public override void Execute(Agent agent)
        {
            // Checking if the agent is null
            if (agent is null)
                return;
            // if agent already has an action to move to a random position && agent is moving towards that direction, return null.
            if (agent.HasAction<Vector3>() && agent.Moving)                    
                return;
            
            // Randomly move microbe in roaming state
            var randomPos = new Vector3(Random.Range(-20f, 20f), agent.transform.position.y, Random.Range(-20f, 20f));
            agent.Move(randomPos);
            agent.Act(randomPos);
            
            // Making agent a microbe variable
            Microbe microbe = agent as Microbe;
            if (microbe is null)
                return;
            
            // When microbe has enough time interval in roaming state
            if (_first && microbe.ElapsedLifespan >= microbe.LifeSpan / 3)
            {
                _first = false;
                microbe.SetState<MicrobeSeekingPickupState>();
            }
            // When microbe gets hungry, move to hungry state
            if (microbe.IsHungry)
                microbe.SetState<MicrobeHungryState>();
            // When microbe is an adult, move to mating state
            if (microbe.IsAdult)
                microbe.SetState<MicrobeMatingState>();
            
        }
        
        public override void Exit(Agent agent)
        {
            agent.Log("Got something to do, stop roaming");
        }
    }
}