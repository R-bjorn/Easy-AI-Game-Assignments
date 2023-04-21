using UnityEngine;
using UnityEngine.AI;

namespace Final_Project.Robot_Types
{
    public class RobotRoamingAI : MonoBehaviour
    {
        
        [SerializeField] private float roamRadius = 10f; // Radius within which to choose random points
        [SerializeField] private float minRoamDelay = 1f; // Minimum delay before choosing a new point
        [SerializeField] private float maxRoamDelay = 5f; // Maximum delay before choosing a new point

        private NavMeshAgent _agent;
        private Vector3 _targetPoint;
        private float _timeToRoam;

        private void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
            ChooseNewTargetPoint();
        }

        private void Update()
        {
            // Check if the agent has arrived at target location or time is out of time bound.
            if (!(_agent.remainingDistance <= _agent.stoppingDistance)) return;
            if (!(Time.time >= _timeToRoam)) return;
            // If the agent has reached its target point, choose a new one after a random delay
            ChooseNewTargetPoint();
        }

        private void ChooseNewTargetPoint()
        {
            // Choose a random point within the roam radius that lies on the navmesh
            Vector3 randomDirection = Random.insideUnitSphere * roamRadius;
            randomDirection += transform.position;
            NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, roamRadius, NavMesh.AllAreas);
            _targetPoint = hit.position;

            // Set the agent's destination to the new target point and reset the roam delay timer
            _agent.SetDestination(_targetPoint);
            _timeToRoam = Time.time + Random.Range(minRoamDelay, maxRoamDelay);
        }
    }
}