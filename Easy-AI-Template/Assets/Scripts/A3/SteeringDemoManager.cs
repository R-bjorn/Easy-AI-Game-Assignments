using EasyAI;
using EasyAI.Navigation;
using UnityEngine;

namespace A3
{
    /// <summary>
    /// Manager that displays some buttons in its controls move the selected agent to various objects.
    /// </summary>
    [DisallowMultipleComponent]
    public class SteeringDemoManager : Manager
    {
        [Header("Steering Demo Parameters")]
        [Tooltip("The objects to list controls for the agent to move in relation to.")]
        [SerializeField]
        private Transform[] targets;
        
        /// <summary>
        /// Render buttons to allow for various move actions to be taken.
        /// </summary>
        /// <param name="x">X rendering position. In most cases this should remain unchanged.</param>
        /// <param name="y">Y rendering position. Update this with every component added and return it.</param>
        /// <param name="w">Width of components. In most cases this should remain unchanged.</param>
        /// <param name="h">Height of components. In most cases this should remain unchanged.</param>
        /// <param name="p">Padding of components. In most cases this should remain unchanged.</param>
        /// <returns>The updated Y position after all custom rendering has been done.</returns>
        protected override float CustomRendering(float x, float y, float w, float h, float p)
        {
            if (SelectedAgent == null)
            {
                return y;
            }

            // Display a button to stop this agent.
            if (GuiButton(x, y, w, h, $"Stop Moving"))
            {
                SelectedAgent.StopMoving();
            }

            // Display buttons to move in relation to all targets.
            foreach (Transform target in targets)
            {
                // Seek the target.
                y = NextItem(y, h, p);
                if (GuiButton(x, y, w, h, $"Seek {target.name}"))
                {
                    SelectedAgent.Move(target);
                }
                
                // Pursue the target.
                y = NextItem(y, h, p);
                if (GuiButton(x, y, w, h, $"Pursue {target.name}"))
                {
                    SelectedAgent.Move(target, Steering.Behaviour.Pursue);
                }
                
                // Flee the target.
                y = NextItem(y, h, p);
                if (GuiButton(x, y, w, h, $"Flee {target.name}"))
                {
                    SelectedAgent.Move(target, Steering.Behaviour.Flee);
                }
                
                // Evade the target.
                y = NextItem(y, h, p);
                if (GuiButton(x, y, w, h, $"Evade {target.name}"))
                {
                    SelectedAgent.Move(target, Steering.Behaviour.Evade);
                }
            }
            
            // Seek back to the origin.
            y = NextItem(y, h, p);
            if (GuiButton(x, y, w, h, "Return to Origin"))
            {
                SelectedAgent.Move(new Vector2(0, 0));
            }

            return NextItem(y, h, p);
        }
    }
}