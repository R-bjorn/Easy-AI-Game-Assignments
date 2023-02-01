using EasyAI;
using UnityEngine;

namespace Project.States
{
    /// <summary>
    /// The global state which soldiers are always in.
    /// </summary>
    [CreateAssetMenu(menuName = "Project/States/Soldier Mind", fileName = "Soldier Mind")]
    public class SoldierMind : State
    {
        public override void Execute(Agent agent)
        {
            // TODO - Project - Create unique behaviours for your soldiers to play capture the flag.
        }
    }
}