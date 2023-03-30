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

        private bool _upLeftCorner;
        private bool _downLeftCorner;
        private bool _upRightCorner;
        private bool _downRightCorner;
    
        /// <summary>
        /// Place nodes at convex corners.
        /// </summary>
        public override void Generate()
        {
            // Get the range of the node area
            var rangeX = NodeArea.RangeX;
            var rangeZ = NodeArea.RangeZ;
            
            // Loop through all the spaces
            for (var x = 0; x < rangeX - 1; x++)
            {
                for (var z = 0; z < rangeZ - 1; z++)
                {
                    // Check if it is a convex corner
                    if (!IsConvexCorner(x, z)) continue;
                    
                    Debug.Log("It is Convex corner");
                    // Place a node at the corner accordingly
                    if(_upLeftCorner)
                        NodeArea.AddNode(x - cornerNodeSteps, z + cornerNodeSteps);
                    else if(_downLeftCorner)
                        NodeArea.AddNode(x + cornerNodeSteps, z + cornerNodeSteps);
                    else if(_upRightCorner)
                        NodeArea.AddNode(x - cornerNodeSteps, z - cornerNodeSteps);
                    else if(_downRightCorner)
                        NodeArea.AddNode(x + cornerNodeSteps, z - cornerNodeSteps);
                    // NodeArea.AddNode(x,z);
                }
            }
        }

        private void ResetBool()
        {
            _upLeftCorner = false;
            _downLeftCorner = false;
            _upRightCorner = false;
            _downRightCorner = false;
        }
        
        /// <summary>
        /// Check if the given space is a convex corner.
        /// </summary>\
        private bool IsConvexCorner(int x, int z)
        {
            ResetBool();
            if (NodeArea.IsOpen(x, z))
                return false;

            if (x == 0 || x == NodeArea.RangeX - 1 || z == 0 || z == NodeArea.RangeZ - 1)
                return false;

            // Get the neighbors of the current space
            var up = NodeArea.IsOpen(x, z + 1);
            var down = NodeArea.IsOpen(x, z - 1);
            var left = NodeArea.IsOpen(x - 1, z);
            var right = NodeArea.IsOpen(x + 1, z);
            
            // Check if the current obstacle element has exactly two neighbor spaces
            var openNeighbor = (up ? 1 : 0) + (down ? 1 : 0) + (left ? 1 : 0) + (right ? 1 : 0);
            if (openNeighbor != 2)
                return false;

            if ( up && down || left && right )
                return false;

            var upLeft = NodeArea.IsOpen(x-1, z+1);
            var upRight = NodeArea.IsOpen(x+1, z+1);
            var downLeft = NodeArea.IsOpen(x-1, z-1);
            var downRight = NodeArea.IsOpen(x+1,z-1);
            
            var openDiagonalNeighbor = (upLeft ? 1 : 0) + (upRight ? 1 : 0) + (downLeft ? 1 : 0) + (downRight ? 1 : 0);
            if (openDiagonalNeighbor != 3)
                return false;
                
            // Check if the diagonal neighbors are spaces
            if (up && left && upLeft)
            {
                _upLeftCorner = true;
                return true;
            }
            if (up && right && upRight)
            {
                _downLeftCorner = true;
                return true;
            }
            if (down && left && downLeft)
            {
                _upRightCorner = true;
                return true;
            }
            if (down && right && downRight)
            {
                _downRightCorner = true;
                return true;
            }
            
            return false;
        }
    }
}