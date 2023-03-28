using UnityEngine;

namespace EasyAI.Navigation.Generators
{
    /// <summary>
    /// Convex corner graph placement for nodes.
    /// </summary>
    public class CornerGraphGenerator : NodeGenerator
    {
        [SerializeField]
        [Min(0)]
        [Tooltip("How far away from corners should the nodes be placed.")]
        private int cornerNodeSteps = 3;
    
        /// <summary>
        /// Place nodes at convex corners.
        /// </summary>
        public override void Generate()
        {
            // Get the range of the node area
            int rangeX = NodeArea.RangeX;
            int rangeZ = NodeArea.RangeZ;
            
            // Loop through all the spaces
            for (int x = 0; x < rangeX; x++)
            {
                for (int z = 0; z < rangeZ; z++)
                {
                    // Check if the space is open
                    if (NodeArea.IsOpen(x, z))
                    {
                        // Check if it is a convex corner
                        if (IsConvexCorner(x, z))
                        {
                            // Place a node at the corner
                            NodeArea.AddNode(x, z);
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// Check if the given space is a convex corner.
        /// </summary>
        private bool IsConvexCorner(int x, int z)
        {
            // Check if there is an obstacle at the space
            if (!NodeArea.IsOpen(x, z))
            {
                return false;
            }
            
            // Check if there are enough free spaces for a corner
            int steps = cornerNodeSteps * NodeArea.nodesPerStep;
            if (!NodeArea.IsOpen(x + steps, z + steps) ||
                !NodeArea.IsOpen(x - steps, z + steps) ||
                !NodeArea.IsOpen(x + steps, z - steps) ||
                !NodeArea.IsOpen(x - steps, z - steps))
            {
                return false;
            }
            
            // Check if the diagonals are clear
            if (NodeArea.IsOpen(x + steps, z + steps) &&
                NodeArea.IsOpen(x - steps, z - steps))
            {
                return false;
            }
            
            if (NodeArea.IsOpen(x + steps, z - steps) &&
                NodeArea.IsOpen(x - steps, z + steps))
            {
                return false;
            }
            
            return true;
        }
    }
}