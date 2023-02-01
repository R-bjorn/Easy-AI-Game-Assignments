using UnityEngine;

namespace EasyAI.Navigation
{
    /// <summary>
    /// Class to hold data for each node during A* pathfinding.
    /// </summary>
    public class AStarNode
    {
        /// <summary>
        /// The position of the node.
        /// </summary>
        public readonly Vector3 Position;

        /// <summary>
        /// The heuristic cost of this node to the goal.
        /// </summary>
        public float CostH { get; }

        /// <summary>
        /// The final cost of this node.
        /// </summary>
        public float CostF => CostG + CostH;

        /// <summary>
        /// The previous node which was moved to prior to this node.
        /// </summary>
        public AStarNode Previous { get; private set; }
    
        /// <summary>
        /// If this node is currently open or closed.
        /// </summary>
        public bool IsOpen { get; private set; }

        /// <summary>
        /// The cost to reach this node from previous nodes.
        /// </summary>
        private float CostG { get; set; }

        /// <summary>
        /// Store node data during A* pathfinding.
        /// </summary>
        /// <param name="pos">The position of the node.</param>
        /// <param name="goal">The goal to find a path to.</param>
        /// <param name="previous">The previous node in the A* pathfinding.</param>
        public AStarNode(Vector3 pos, Vector3 goal, AStarNode previous = null)
        {
            Open();
            Position = pos;
            CostH = Vector3.Distance(Position, goal);
            UpdatePrevious(previous);
        }

        /// <summary>
        /// Update the node to have a new previous node and then update its G cost.
        /// </summary>
        /// <param name="previous">The previous node in the A* pathfinding.</param>
        public void UpdatePrevious(AStarNode previous)
        {
            Previous = previous;
            if (Previous == null)
            {
                CostG = 0;
                return;
            }

            CostG = previous.CostG + Vector3.Distance(Position, Previous.Position);
        }

        /// <summary>
        /// Open the node.
        /// </summary>
        public void Open()
        {
            IsOpen = true;
        }

        /// <summary>
        /// Close the node.
        /// </summary>
        public void Close()
        {
            IsOpen = false;
        }
    }
}