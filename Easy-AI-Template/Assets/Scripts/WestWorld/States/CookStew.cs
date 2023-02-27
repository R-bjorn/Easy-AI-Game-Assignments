using EasyAI;
using UnityEngine;
using WestWorld.Agents;

namespace WestWorld.States
{
    /// <summary>
    /// State for the house keeper to cook stew in.
    /// </summary>
    [CreateAssetMenu(menuName = "West World/States/Cook Stew State", fileName = "Cook Stew State")]
    public class CookStew : State
    {
        public override void Enter(Agent agent)
        {
            agent.Log("Puttin' the stew in the oven.");
        }

        public override void Execute(Agent agent)
        {
            agent.Log("Fussin' over food.");
            
            // Stew has a one in five chance of being ready.
            if (new System.Random().Next(5) == 0)
            {
                // Return to doing housework when the stew is done.
                agent.SetState<DoHousework>();
            }
        }

        public override void Exit(Agent agent)
        {
            HouseKeeper houseKeeper = agent as HouseKeeper;
            houseKeeper.Log("Stew ready! Let's eat.");
            
            // Tell the miner that the stew is ready.
            houseKeeper.SendMessage(WestWorldAgent.WestWorldMessage.StewReady);
            
            houseKeeper.Log("Puttin' the stew on the table.");
        }
    }
}