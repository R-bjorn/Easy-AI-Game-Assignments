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
            var openSet = new List<AStarNode>(); // The set of currently discovered nodes that are not evaluated yet.
            var closedSet = new List<AStarNode>(); // The set of nodes already evaluated.
            var startNode = new AStarNode(current, goal); // Create a starting node and calculate its cost.

            openSet.Add(startNode); // Add the starting node to the open set.

            while (openSet.Count > 0)
            {
                var currentNode = openSet[0];

                // Check if we have reached the goal.
                if (currentNode.Position == goal)
                {
                    return ReconstructPath(currentNode); // Return the path to the goal.
                }

                openSet.Remove(currentNode); // Remove the current node from the open set.
                closedSet.Add(currentNode); // Add the current node to the closed set.

                // Loop through all connections of the current node.
                foreach (var connection in connections)
                {
                    // Check if the current connection is connected to the current node.
                    if (connection.A != currentNode.Position)
                    {
                        continue;
                    }

                    var neighbour = new AStarNode(connection.B, goal, currentNode); // Create a new neighbour node.

                    // Check if the neighbour node is in the closed set.
                    if (closedSet.Contains(neighbour))
                    {
                        continue;
                    }

                    // Calculate the tentative G score for the neighbour node.
                    var tentativeGScore = currentNode.CostG + connection.Cost;
                    // Check if the neighbour node is in the open set or not.
                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour); // Add the neighbour node to the open set.
                    }
                    else if (tentativeGScore >= neighbour.CostG)
                    {
                        continue; // This is not a better path.
                    }

                    // This path is the best until now, record it.
                    neighbour.UpdatePrevious(currentNode);
                    neighbour.Close();
                }
            }

            // If there is no path found, return an empty list.
            return new List<Vector3>();
        }
        
        /// <summary>
        /// Reconstructs the path from the end node to the start node.
        /// </summary>
        /// <param name="endNode">The end node of the path.</param>
        /// <returns>The path of nodes to take to get from the starting position to the ending position.</returns>
        private static List<Vector3> ReconstructPath(AStarNode endNode)
        {
            var path = new List<Vector3>();
            var currentNode = endNode;

            while (currentNode != null)
            {
                path.Add(currentNode.Position);
                currentNode = currentNode.Previous;
            }

            // Reverse the path so that it starts from the start position and ends at the end position
            path.Reverse();

            return path;
        }
    }
}