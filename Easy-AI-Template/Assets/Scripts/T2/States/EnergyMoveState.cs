using EasyAI;
using T2.Actions;
using T2.Sensors;
using UnityEngine;

namespace T2.States
{
    /// <summary>
    /// The state which the energy demo agent moves in.
    /// </summary>
    [CreateAssetMenu(menuName = "T2/States/Energy Move State", fileName = "Energy Move State")]
    public class EnergyMoveState : State
    {
        public override void Enter(Agent agent)
        {
            agent.Log("Ready to move.");
        }

        public override void Execute(Agent agent)
        {
            agent.Log("Moving randomly to burn this energy.");
            Vector2 random = Random.insideUnitCircle;
            agent.Move(agent.transform.position + new Vector3(random.x, 0, random.y));
            
            // Create deplete energy action.
            agent.Act(new DepleteEnergyAction(agent.Sense<EnergySensor, EnergyComponent>()));
        }

        public override void Exit(Agent agent)
        {
            agent.Log("Been moving for a while, getting tired.");
        }
    }
}