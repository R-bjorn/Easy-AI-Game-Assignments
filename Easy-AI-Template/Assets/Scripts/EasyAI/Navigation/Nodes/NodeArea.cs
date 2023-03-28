using System.Collections.Generic;
using System.IO;
using System.Linq;
using EasyAI.Navigation.Generators;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EasyAI.Navigation.Nodes
{
    /// <summary>
    /// Define an area for nodes to be placed in by an associated node generator.
    /// </summary>
    public class NodeArea : NodeBase
    {
        /// <summary>
        /// Open symbol.
        /// </summary>
        private const char Open = ' ';
    
        /// <summary>
        /// Closed symbol.
        /// </summary>
        private const char Closed = '#';

        /// <summary>
        /// Node symbol.
        /// </summary>
        private const char Node = '*';

        [SerializeField]
        [Tooltip("One of the corner coordinates (X, Z) of the area to generate nodes in.")]
        private int2 corner1 = new(5, 5);
    
        [SerializeField]
        [Tooltip("One of the corner coordinates (X, Z) of the area to generate nodes in.")]
        private int2 corner2 = new(-5, -5);

        [SerializeField]
        [Tooltip("The floor and ceiling to cast down between.")]
        private float2 floorCeiling = new(-1, 10);

        [SerializeField]
        [Min(1)]
        [Tooltip(
            "How many nodes to place for every unit of world space. Example values:\n" +
            "1 - Node per every 1 unit.\n" +
            "2 - Node per every 0.5 units.\n" +
            "4 - Node per every 0.25 units."
        )]
        private int nodesPerStep = 4;

        /// <summary>
        /// How many node spaces there are on the X axis.
        /// </summary>
        public int RangeX => (corner1.x - corner2.x) * nodesPerStep + 1;
    
        /// <summary>
        /// How many node spaces there are on the Z axis.
        /// </summary>
        public int RangeZ => (corner1.y - corner2.y) * nodesPerStep + 1;

        /// <summary>
        /// Data map.
        /// </summary>
        private char[,] _data;

        /// <summary>
        /// The nodes.
        /// </summary>
        private readonly List<Vector3> _nodes = new();

        private void OnDrawGizmosSelected()
        {
            // Vertical lines.
            Gizmos.DrawLine(new(corner1.x, floorCeiling.x, corner1.y), new(corner1.x, floorCeiling.y, corner1.y));
            Gizmos.DrawLine(new(corner1.x, floorCeiling.x, corner2.y), new(corner1.x, floorCeiling.y, corner2.y));
            Gizmos.DrawLine(new(corner2.x, floorCeiling.x, corner1.y), new(corner2.x, floorCeiling.y, corner1.y));
            Gizmos.DrawLine(new(corner2.x, floorCeiling.x, corner2.y), new(corner2.x, floorCeiling.y, corner2.y));
        
            // Top horizontal lines.
            Gizmos.DrawLine(new(corner1.x, floorCeiling.y, corner1.y), new(corner1.x, floorCeiling.y, corner2.y));
            Gizmos.DrawLine(new(corner1.x, floorCeiling.y, corner1.y), new(corner2.x, floorCeiling.y, corner1.y));
            Gizmos.DrawLine(new(corner2.x, floorCeiling.y, corner2.y), new(corner1.x, floorCeiling.y, corner2.y));
            Gizmos.DrawLine(new(corner2.x, floorCeiling.y, corner2.y), new(corner2.x, floorCeiling.y, corner1.y));
        
            // Bottom horizontal lines.
            Gizmos.DrawLine(new(corner1.x, floorCeiling.x, corner1.y), new(corner1.x, floorCeiling.x, corner2.y));
            Gizmos.DrawLine(new(corner1.x, floorCeiling.x, corner1.y), new(corner2.x, floorCeiling.x, corner1.y));
            Gizmos.DrawLine(new(corner2.x, floorCeiling.x, corner2.y), new(corner1.x, floorCeiling.x, corner2.y));
            Gizmos.DrawLine(new(corner2.x, floorCeiling.x, corner2.y), new(corner2.x, floorCeiling.x, corner1.y));
        }

        public List<Vector3> Generate()
        {
            // Ensure X coordinates are in the required order.
            if (corner2.x > corner1.x)
            {
                (corner1.x, corner2.x) = (corner2.x, corner1.x);
            }
        
            // Ensure Z coordinates are in the required order.
            if (corner2.y > corner1.y)
            {
                (corner1.y, corner2.y) = (corner2.y, corner1.y);
            }

            // Ensure floor and ceiling are in the required order.
            if (floorCeiling.x > floorCeiling.y)
            {
                (floorCeiling.x, floorCeiling.y) = (floorCeiling.y, floorCeiling.x);
            }

            // Initialize the data table.
            _data = new char[RangeX, RangeZ];
        
            // Scan each position to determine if it is open or closed.
            for (int x = 0; x < RangeX; x++)
            {
                for (int z = 0; z < RangeZ; z++)
                {
                    float2 pos = GetRealPosition(x, z);
                    _data[x, z] = Physics.Raycast(new(pos.x, floorCeiling.y, pos.y), Vector3.down, out RaycastHit hit, floorCeiling.y - floorCeiling.x, Manager.GroundLayers | Manager.ObstacleLayers) && (Manager.GroundLayers.value & (1 << hit.transform.gameObject.layer)) > 0
                        ? Open
                        : Closed;
                }
            }
        
            // Get the node generator.
            List<NodeGenerator> generators = GetComponents<NodeGenerator>().ToList();
            generators.AddRange(GetComponentsInChildren<NodeGenerator>());
            foreach (NodeGenerator generator in generators)
            {
                // Run the node generator.
                generator.NodeArea = this;
                generator.Generate();
            }

#if UNITY_EDITOR
            // Ensure the folder to save the map data exists.
            const string folder = "Maps";
            if (!Directory.Exists(folder))
            {
                DirectoryInfo info = Directory.CreateDirectory(folder);
                if (!info.Exists)
                {
                    return _nodes;
                }
            }
    
            // Write to the file.
            string fileName = $"{folder}/{SceneManager.GetActiveScene().name}";
            NodeArea[] levelSections = FindObjectsOfType<NodeArea>();
            if (levelSections.Length > 1)
            {
                fileName += $"_{levelSections.ToList().IndexOf(this)}";
            }
            fileName += ".txt";
    
            StreamWriter writer = new(fileName, false);
            writer.Write(ToString());
            writer.Close();
#endif
            return _nodes;
        }

        /// <summary>
        /// Get the actual coordinate from the node generator indexes.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="z">The Z coordinate.</param>
        /// <returns>The real (X, Z) position.</returns>
        private float2 GetRealPosition(int x, int z)
        {
            return new(corner2.x + x * 1f / nodesPerStep, corner2.y + z * 1f / nodesPerStep);
        }

        /// <summary>
        /// Check if a given coordinate is open.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="z">The Z coordinate.</param>
        /// <returns>True if the space is open, false otherwise.</returns>
        public bool IsOpen(int x, int z)
        {
            return _data[x, z] != Closed;
        }

        /// <summary>
        /// Add a node at a given position.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="z"></param>
        public void AddNode(int x, int z)
        {
            // Set that it is a node in the map data.
            _data[x, z] = Node;
        
            // Get the position of the node.
            float2 pos = GetRealPosition(x, z);
            float y = floorCeiling.x;
            if (Physics.Raycast(new(pos.x, floorCeiling.y, pos.y), Vector3.down, out RaycastHit hit, floorCeiling.y - floorCeiling.x, Manager.GroundLayers))
            {
                y = hit.point.y;
            }
        
            // Add the node.
            Vector3 v = new(pos.x, y, pos.y);
            if (!_nodes.Contains(v))
            {
                _nodes.Add(v);
            }
        }

        public override string ToString()
        {
            // Nothing to write if there is no data.
            if (_data == null)
            {
                return "No data.";
            }

            // Add all map data.
            string s = string.Empty;
            for (int i = 0; i < RangeX; i++)
            {
                for (int j = 0; j < RangeZ; j++)
                {
                    s += _data[i, j];
                }

                if (i != RangeX - 1)
                {
                    s += '\n';
                }
            }

            return s;
        }
    }
}