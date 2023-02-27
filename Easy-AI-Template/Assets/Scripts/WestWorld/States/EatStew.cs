using EasyAI;
using UnityEngine;

namespace WestWorld.States
{
    /// <summary>
    /// State for the miner to eat stew.
    /// </summary>
    [CreateAssetMenu(menuName = "West World/States/Eat Stew State", fileName = "Eat Stew State")]
    public class EatStew : State
    {
        public override void Enter(Agent agent)
        {
            agent.Log("Okay hun, ahm a-comin'!");
            agent.Log("Smells reaaal goood, Elsa!");
        }

        public override void Execute(Agent agent)
        {
            agent.Log("Tastes real good too!");
            
            // After eating, go back to resting.
            agent.SetState<GoHomeAndSleepTillRested>();
        }

        public override void Exit(Agent agent)
        {
            agent.Log("Thank ya li'l lady. Ah better get back to whatever ah wuz doin'.");
        }
    }
}