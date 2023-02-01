using UnityEngine;

namespace EasyAI.Navigation.Nodes
{
    /// <summary>
    /// Hold a connection between two nodes.
    /// </summary>
    public struct Connection
    {
        /// <summary>
        /// A node in the connection.
        /// </summary>
        public readonly Vector3 A;
        
        /// <summary>
        /// A node in the connection.
        /// </summary>
        public readonly Vector3 B;

        /// <summary>
        /// Add a connection for two nodes.
        /// </summary>
        /// <param name="a">The first node.</param>
        /// <param name="b">The second node.</param>
        public Connection(Vector3 a, Vector3 b)
        {
            A = a;
            B = b;
        }
    }
}