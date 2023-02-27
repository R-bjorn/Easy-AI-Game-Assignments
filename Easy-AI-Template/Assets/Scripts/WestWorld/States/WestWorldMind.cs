using EasyAI;
using UnityEngine;
using WestWorld.Agents;

namespace WestWorld.States
{
    /// <summary>
    /// The state which the miner and house keeper are always in.
    /// </summary>
    [CreateAssetMenu(menuName = "West World/States/West World Mind", fileName = "West World Mind")]
    public class WestWorldMind : State
    {
        public override void Enter(Agent agent)
        {
            // Start the miner at the mine and the house keeper doing housework.
            if (agent is Miner)
            {
                agent.SetState<EnterMineAndDigForNugget>();
            }
            else
            {
                agent.SetState<DoHousework>();
            }
        }

        public override void Execute(Agent agent)
        {
            switch (agent)
            {
                // The house keeper has a one in ten chance to need to go to the bathroom.
                case HouseKeeper houseKeeper:
                    if (new System.Random().Next(10) == 0)
                    {
                        // Save the last state so the house keeper knows what to return to.
                        houseKeeper.SaveLastState();
                        
                        // Go to the bathroom.
                        houseKeeper.SetState<VisitBathroom>();
                    }
                    return;
                
                // The miner always has their thirst increase.
                case Miner miner:
                    miner.IncreaseThirst();
                    break;
            }
        }
    }
}