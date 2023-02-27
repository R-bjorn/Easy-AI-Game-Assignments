using EasyAI;
using UnityEngine;
using WestWorld.Agents;

namespace WestWorld.States
{
    /// <summary>
    /// State for the house keeper to head to the bathroom.
    /// </summary>
    [CreateAssetMenu(menuName = "West World/States/Visit Bathroom State", fileName = "Visit Bathroom State")]
    public class VisitBathroom : State
    {
        public override void Enter(Agent agent)
        {
            agent.Log("Elsa: Walkin' to the can. Need to powda mah pretty li'l nose");
        }

        public override void Execute(Agent agent)
        {
            HouseKeeper houseKeeper = agent as HouseKeeper;
            houseKeeper.Log("Elsa: Ahhhhhh! Sweet relief!");
            
            // Once done in the bathroom, return to whatever they were doing last.
            houseKeeper.ReturnToLastState();
        }

        public override void Exit(Agent agent)
        {
            agent.Log("Leavin' the john.");
        }
    }
}