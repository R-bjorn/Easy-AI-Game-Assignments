using System.Collections.Generic;
using EasyAI.Navigation.Nodes;
using UnityEngine;

namespace EasyAI.Navigation
{
    /// <summary>
    /// A* pathfinding.
    /// </summary>
    public static class AStar
    {
        /// <summary>
        /// Perform A* pathfinding.
        /// </summary>
        /// <param name="current">The starting position.</param>
        /// <param name="goal">The end goal position.</param>
        /// <param name="connections">All node connections in the scene.</param>
        /// <returns>The path of nodes to take to get from the starting position to the ending position.</returns>
        public static List<Vector3> Perform(Vector3 current, Vector3 goal, List<Connection> connections)
        {
            // Create the open and closed lists
            var openList = new List<AStarNode>();
            var closedList = new List<AStarNode>();

            // Create the starting node
            var startNode = new AStarNode(current, goal);

            // Add the starting node to the open list
            openList.Add(startNode);

            // Loop until the open list is empty
            while (openList.Count > 0)
            {
                // Sort the open list by cost F (lowest to highest)
                openList.Sort((node1, node2) => node1.CostF.CompareTo(node2.CostF));

                // Select the node with the lowest cost F
                var  currentNode = openList[0];

                // If the current node is the goal node, we are done
                if (currentNode.Position == goal)
                {
                    // Reconstruct the path
                    var path = new List<Vector3>();
                    while (currentNode != null)
                    {
                        path.Add(currentNode.Position);
                        currentNode = currentNode.Previous;
                    }
                    path.Reverse();
                    return path;
                }

                // Move the current node from the open list to the closed list
                openList.Remove(currentNode);
                closedList.Add(currentNode);

                // Get the successors of the current node
                foreach (var connection in connections)
                {
                    if (connection.A != currentNode.Position) continue;
                    var successorNode = new AStarNode(connection.B, goal, currentNode);

                    // Check if the successor node is already on the closed list
                    if (closedList.Contains(successorNode))
                        continue;

                    // Check if the successor node is already on the open list
                    var existingNode = openList.Find(node => node.Position == successorNode.Position);
                    if (existingNode != null)
                    {
                        // If the new path to the successor node is shorter, update the G cost and previous node
                        if (successorNode.CostG < existingNode.CostG)
                        {
                            existingNode.UpdatePrevious(currentNode);
                        }
                    }
                    else
                    {
                        // Add the successor node to the open list
                        openList.Add(successorNode);
                    }
                }
            }

            // If we get here, there is no path to the goal
            return null;
        }
        
    }
}