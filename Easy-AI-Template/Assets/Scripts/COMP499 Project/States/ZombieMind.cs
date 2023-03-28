using COMP499_Project.States.Zombie_Basic_Behaviour;
using EasyAI;
using UnityEngine;

namespace COMP499_Project.States
{
    [SerializeField]
    [CreateAssetMenu(menuName = "COMP499 Project/States/Zombie Mind", fileName = "Zombie Mind")]
    public class ZombieMind : State
    {
        public override void Enter(Agent agent)
        {
            // Initial roaming state
            if (agent is null)
                return;
            agent.SetState<ZombieRoamingState>();
        }

        public override void Execute(Agent agent)
        {
            return;
        }
    }
}
