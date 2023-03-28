using EasyAI;

namespace COMP499_Project.States.Zombie_Basic_Behaviour
{
    public class ZombieRestingState : State
    {
        public override void Enter(Agent agent)
        {
            agent.Log("");
        }

        public override void Execute(Agent agent)
        {
            // Resting and charging battery life
            
            // if zombie sensors catches player
            // Zombie moves to pursue player state
            
            // if zombie battery is recharged
            // Zombie moves to roaming state
        }
        
        public override void Exit(Agent agent)
        {
            agent.Log("");
        }
    }
}
