using EasyAI;
using UnityEngine;
using WestWorld.Agents;

namespace WestWorld.States
{
    /// <summary>
    /// State for the miner to drink.
    /// </summary>
    [CreateAssetMenu(menuName = "West World/States/Quench Thirst State", fileName = "Quench Thirst State")]
    public class QuenchThirst : State
    {
        public override void Enter(Agent agent)
        {
            Miner miner = agent as Miner;;

            if (miner.Location == Miner.WestWorldLocation.Saloon)
            {
                return;
            }
            
            // Update the location to the saloon.
            miner.ChangeLocation(Miner.WestWorldLocation.Saloon);
            miner.Log("Boy, ah sure is thusty! Walkin' to the saloon");
        }

        public override void Execute(Agent agent)
        {
            Miner miner = agent as Miner;
            
            // Drink up.
            miner.Drink();
            miner.Log("That's mighty fine sippin liquor.");
            
            // Start heading back to the mines.
            miner.SetState<EnterMineAndDigForNugget>();
        }

        public override void Exit(Agent agent)
        {
            agent.Log("Leavin' the saloon, feelin' good.");
        }
    }
}