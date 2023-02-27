using EasyAI;
using UnityEngine;
using WestWorld.Agents;

namespace WestWorld.States
{
    /// <summary>
    /// State for the miner to deposit their gold.
    /// </summary>
    [CreateAssetMenu(menuName = "West World/States/Visit Bank And DepositGold State", fileName = "Visit Bank And Deposit Gold State")]
    public class VisitBankAndDepositGold : State
    {
        public override void Enter(Agent agent)
        {
            Miner miner = agent as Miner;;

            if (miner.Location == Miner.WestWorldLocation.Bank)
            {
                return;
            }
            
            // Update the location to the bank.
            miner.ChangeLocation(Miner.WestWorldLocation.Bank);
            miner.Log("Goin' to the bank. Yes siree.");
        }

        public override void Execute(Agent agent)
        {
            Miner miner = agent as Miner;

            // Deposit all gold to the bank.
            miner.DepositGold();
            miner.Log($"Depositin’ gold. Total savings now: {miner.MoneyInBank}");

            // If the miner still has energy, head back to the mine.
            if (!miner.Tired)
            {
                miner.SetState<EnterMineAndDigForNugget>();
                return;
            }

            // Otherwise, head home to rest.
            miner.Log("Woohoo! Rich enough for now. Back home to mah li'l lady.");
            miner.SetState<GoHomeAndSleepTillRested>();
        }

        public override void Exit(Agent agent)
        {
            agent.Log("Leavin' the bank.");
        }
    }
}