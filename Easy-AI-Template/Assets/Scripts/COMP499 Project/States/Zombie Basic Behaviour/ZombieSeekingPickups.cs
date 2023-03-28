using EasyAI;

namespace COMP499_Project.States.Zombie_Basic_Behaviour
{
    public class ZombieSeekingPickups : State
    {
        public override void Enter(Agent agent)
        {
            agent.Log("");
        }

        public override void Execute(Agent agent)
        {
            // Collecting the nearest pickup
            
            // if zombie sensors catches player
            // Zombie moves to pursue player state
            
            // if zombie collected the pickup
            // Zombie moves to Roaming state
        }
        
        public override void Exit(Agent agent)
        {
            agent.Log("");
        }
    }
}
