using EasyAI;
using UnityEngine;
using WestWorld.Agents;

namespace WestWorld.States
{
    /// <summary>
    /// State for the miner to collect gold.
    /// </summary>
    [CreateAssetMenu(menuName = "West World/States/Enter Mine And Dig For Nugget State", fileName = "Enter Mine And Dig For Nugget State")]
    public class EnterMineAndDigForNugget : State
    {
        public override void Enter(Agent agent)
        {
            Miner miner = agent as Miner;;

            if (miner.Location == Miner.WestWorldLocation.GoldMine)
            {
                return;
            }

            // Update the location to the gold mine.
            miner.ChangeLocation(Miner.WestWorldLocation.GoldMine);
            miner.Log("Walkin' to the gold mine.");
        }

        public override void Execute(Agent agent)
        {
            Miner miner = agent as Miner;

            // Work and collect gold.
            miner.IncreaseFatigue();
            miner.AddToGoldCarried();
            miner.Log("Pickin' up a nugget.");

            // If full on gold, start to deposit.
            if (miner.PocketsFull)
            {
                miner.SetState<VisitBankAndDepositGold>();
                return;
            }

            // Otherwise if thirsty, start to quench thirst.
            if (miner.Thirsty)
            {
                miner.SetState<QuenchThirst>();
            }
        }

        public override void Exit(Agent agent)
        {
            agent.Log("Ah'm leavin' the gold mine with mah pockets full o' sweet gold.");
        }
    }
}