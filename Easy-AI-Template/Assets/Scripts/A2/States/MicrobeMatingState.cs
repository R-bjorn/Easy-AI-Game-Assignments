using A2.Sensors;
using EasyAI;
using UnityEngine;

namespace A2.States
{
    /// <summary>
    /// State for microbes that are seeking a mate.
    /// </summary>
    [CreateAssetMenu(menuName = "A2/States/Microbe Mating State", fileName = "Microbe Mating State")]
    public class MicrobeMatingState : State
    {
        // private Microbe _prevMate;
        public override void Enter(Agent agent)
        {
            agent.Log("Looking for a mate.");
        }
        
        public override void Execute(Agent agent)
        {
            if (agent is not Microbe m1)
                return;
            // find a nearest mate by using sensors. 
            var m2 = m1.Sense<NearestMateSensor, Microbe>();
            if (m2 is null)
            {
                // if agent already has an action to move to a random position && agent is moving towards that direction, return null.
                if (agent.HasAction<Vector3>() && agent.Moving)                    
                    return;
                // Randomly move microbe in roaming state
                var randomPos = new Vector3(Random.Range(-20f, 20f), agent.transform.position.y, Random.Range(-20f, 20f));
                agent.Move(randomPos);
                agent.Act(randomPos);
                return;
            }

            // if m1 microbe doesn't already have a target microbe, attract the nearest microbe
            if(!m1.HasTarget)
                m1.AttractMate(m2);

            // While mating hasn't fully done, move to the nearest microbe
            if (m1.HasTarget && !m1.Mate())
            {
                m1.Move(m2.transform);
                m1.Act(m2);
            }
            else
                m1.SetState<MicrobeRoamingState>();
        }
        
        public override void Exit(Agent agent)
        {
            agent.Log("No longer looking for a mate.");
        }
    }
}