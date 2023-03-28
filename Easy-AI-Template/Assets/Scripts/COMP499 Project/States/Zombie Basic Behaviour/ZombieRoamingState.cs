using COMP499_Project.Game_Scripts;
using EasyAI;
using UnityEngine;

namespace COMP499_Project.States.Zombie_Basic_Behaviour
{
    public class ZombieRoamingState : State
    {
        public override void Enter(Agent agent)
        {
            agent.Log("");
        }

        public override void Execute(Agent agent)
        {
            // Random movement of the zombie character

            // After energy is low, after time interval
            // Zombie Moves to Resting State or Collecting Pickup State -> Random coin toss pickup
            
            // If zombie detects player from sensors && senses player threat
            // Zombie moves to Evade from player state
            
            // Else
            // Zombie moves to Pursue Player State 
        }
        
        public override void Exit(Agent agent)
        {
            agent.Log("");
        }
    }
}
