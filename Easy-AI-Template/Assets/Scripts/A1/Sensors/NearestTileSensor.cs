using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using EasyAI;

namespace A1.Sensors
{
    /// <summary>
    /// Senor to sense the nearest dirty tile to the agent.
    /// </summary>
    [DisallowMultipleComponent]
    public class NearestTileSensor : Sensor
    {
        public override object Sense()
        {

            List<Transform> dirtyTiles = new();
            foreach (Transform tile in FindObjectsOfType<Transform>().Where(t => t.name.Contains("Floor")).ToArray())
            {
                if (tile.gameObject.GetComponent<Floor>().IsDirty)
                    dirtyTiles.Add(tile);
            }


            if (dirtyTiles.Count == 0)
            {
                Log("No dirty tiles left, All tiles are cleaned!");
                return null;
            }

            Log("Getting nearest dirty tile");
            return dirtyTiles.OrderBy(b => Vector3.Distance(Agent.transform.position, b.transform.position)).First();
        }
    }
}
