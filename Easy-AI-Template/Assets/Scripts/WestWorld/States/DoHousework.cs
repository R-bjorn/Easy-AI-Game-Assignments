using EasyAI;
using UnityEngine;

namespace WestWorld.States
{
    /// <summary>
    /// State for the house keeper to clean the house in.
    /// </summary>
    [CreateAssetMenu(menuName = "West World/States/Do Housework State", fileName = "Do Housework State")]
    public class DoHousework : State
    {
        public override void Enter(Agent agent)
        {
            agent.Log("Time to do some more housework!");
        }

        public override void Execute(Agent agent)
        {
            // Randomly clean something, or a chance nothing is done.
            switch (new System.Random().Next(4))
            {
                case 3:
                    agent.Log("Washin' the dishes.");
                    break;
                case 2:
                    agent.Log("Makin' the bed.");
                    break;
                case 1:
                    agent.Log("Moppin' the floor.");
                    break;
            }
        }
    }
}