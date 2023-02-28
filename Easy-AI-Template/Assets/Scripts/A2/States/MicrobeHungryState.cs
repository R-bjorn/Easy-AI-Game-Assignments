using A2.Sensors;
using EasyAI;
using EasyAI.Navigation;
using UnityEngine;

namespace A2.States
{
    /// <summary> 
    /// State for microbes that are hungry and wanting to seek food.
    /// </summary>
    [CreateAssetMenu(menuName = "A2/States/Microbe Hungry State", fileName = "Microbe Hungry State")]
    public class MicrobeHungryState : State
    {
        public override void Enter(Agent agent)
        {
            agent.Log("Feeling hungry!!");
        }
        
        public override void Execute(Agent agent)
        {
            
            if (agent is null || agent.HasAction<Transform>())
            {
                return;
            }
            
            //Getting variables
            Microbe microbe = agent as Microbe;
            if (microbe is null)
                return;
            Microbe preyMicrobe = microbe.Sense<NearestPreySensor, Microbe>();
            if (preyMicrobe is null)
            {   
                agent.Log("No microbes to eat.");
                agent.SetState<MicrobeRoamingState>();
                return;
            }

            if (preyMicrobe.HasTarget)
                return;
            // start hunting the prey microbe
            microbe.StartHunting(preyMicrobe);
            preyMicrobe.SetState<MicrobeHuntedState>();
            
            // While microbe hasn't ate any other microbe, keep moving towards them. otherwise, move to roaming state
            if (!microbe.Eat()){
                microbe.Move(preyMicrobe.transform, Steering.Behaviour.Pursue);
                microbe.Act(preyMicrobe);
            }
            else
                microbe.SetState<MicrobeRoamingState>();
        }
        
        public override void Exit(Agent agent)
        {
            agent.Log("Had enough to eat. uffff!!");
        }
    }
}