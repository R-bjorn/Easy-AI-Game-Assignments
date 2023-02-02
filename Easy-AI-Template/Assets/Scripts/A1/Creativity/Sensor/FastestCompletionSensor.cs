using System.Collections.Generic;
using System.Linq;
using EasyAI;
using UnityEngine;

namespace A1.Creativity
{
    /// <summary>
    /// Senor to sense the nearest dirty tile to the agent.
    /// </summary>
    [DisallowMultipleComponent]
    public class FastestCompletionSensor : Sensor
    {
        public override object Sense()
        {
            Transform[] obstacles = FindObjectsOfType<Transform>().Where(t => t.CompareTag("Pickups")).ToArray();
            List<Transform> pickedupObstacles = obstacles.Where(t=>!(t.GetComponent<PickupObstacles>().isNotPickedUp)).ToList();

            if (obstacles.Length == 0)
            {
                Log("No dirty tiles left, All tiles are cleaned!");
                return null;
            }
            
            // If Player has already picked up the obstacle, now sense where to drop the obstacle.
            if (pickedupObstacles.Count > 0)
            {
                Transform destCollector = null;
                Transform[] collectorList =
                    FindObjectsOfType<Transform>().Where(t => t.CompareTag("Collectors")).ToArray().OrderBy(b => Vector3.Distance(Agent.transform.position, b.transform.position)).ToArray();;


                if (pickedupObstacles.First().name.Contains("Cube"))
                {
                    destCollector = collectorList.Where(t => t.name.Contains("Cube")).ToArray().First();
                }
                if (pickedupObstacles.First().name.Contains("Sphere"))
                {
                    destCollector = collectorList.Where(t => t.name.Contains("Sphere")).ToArray().First();
                }
                if (pickedupObstacles.First().name.Contains("Cylinder"))
                {
                    destCollector = collectorList.Where(t => t.name.Contains("Cylinder")).ToArray().First();
                }
                
                // Log("Getting nearest Collector");
                return destCollector;
            }

            // Otherwise move to the next nearest pickup position.
            // Log("Getting nearest obstacle");
            return obstacles.OrderBy(b => Vector3.Distance(Agent.transform.position, b.transform.position)).First();
            // Dictionary<Transform, Transform> tasksAssigned = new Dictionary<Transform, Transform>();
            //
            // 
            //
            // foreach (Transform obj in FindObjectsOfType<Transform>().Where(t => t.name.Contains("Pickup")).ToArray())
            // {
            //     if (obj.name.Contains("Cube"))
            //         tasksAssigned.Add(obj, cubeCollector);
            //     else if (obj.name.Contains("Sphere"))
            //         tasksAssigned.Add(obj, sphereCollector);
            //     else if (obj.name.Contains("Cylinder"))
            //         tasksAssigned.Add(obj, cylinderCollector);
            // }
            //
            // if (tasksAssigned.Count == 0)
            // {
            //     Log("No tasks given!");
            //     return null;
            // }
            //
            // double minDistance = 1000;
            // KeyValuePair<Transform, Transform> firstTask = new KeyValuePair<Transform, Transform>();
            // foreach (var task in tasksAssigned)
            // {
            //     double taskDistance = Vector3.Distance(task.Key.transform.position, task.Value.transform.position);
            //     if (taskDistance < minDistance)
            //     {
            //         firstTask = task;
            //         minDistance = taskDistance;
            //     }
            //         
            //     
            // }
            //
            // Log("Getting the task.");
            // return firstTask.Key;
        }
    }
}

