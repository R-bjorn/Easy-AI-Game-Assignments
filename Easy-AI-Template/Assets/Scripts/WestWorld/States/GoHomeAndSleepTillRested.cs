using EasyAI;
using UnityEngine;
using WestWorld.Agents;

namespace WestWorld.States
{
    /// <summary>
    /// State for the miner to recharge at home.
    /// </summary>
    [CreateAssetMenu(menuName = "West World/States/Go Home And Sleep Till Rested State", fileName = "Go Home And Sleep Till Rested State")]
    public class GoHomeAndSleepTillRested : State
    {
        public override void Enter(Agent agent)
        {
            Miner miner = agent as Miner;

            if (miner.Location == Miner.WestWorldLocation.Home)
            {
                return;
            }
            
            // Update the location to the home.
            miner.ChangeLocation(Miner.WestWorldLocation.Home);
            miner.Log("Walkin' home.");
            
            // Tell the house keeper they are home.
            miner.SendMessage(WestWorldAgent.WestWorldMessage.HiHoneyImHome);
        }

        public override void Execute(Agent agent)
        {
            Miner miner = agent as Miner;
            
            // Rest up.
            miner.Rest();
            miner.Log("ZZZZ...");

            // Start heading back to the mines once fully rested.
            if (miner.Rested)
            {
                miner.SetState<EnterMineAndDigForNugget>();
            }
        }

        public override void Exit(Agent agent)
        {
            agent.Log("What a God-darn fantastic nap! Time to find more gold.");
        }
    }
}