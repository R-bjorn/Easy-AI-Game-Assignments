using Final_Project.Sensors;
using UnityEngine;
using UnityEngine.AI;

namespace Final_Project.Robot_Types
{
    public class MedicRobotAI : MonoBehaviour
    {
        private NearestHealingStation _healingStationSensor;
    
        // The distance at which the player can pick up the weapon
        public float healingDistance = 0.2f;
    
        private NavMeshAgent _agent;
    
        // Whether the player is currently moving towards the weapon
        private bool _movingTowardStation;
        
        // Weapon pickup target
        private Transform _targetPos;
    
        private void Start()
        {
            _agent = GetComponent<NavMeshAgent>();

            _healingStationSensor = _agent.GetComponent<NearestHealingStation>();

            MoveTowardsHealingStation();
        }

        private void MoveTowardsHealingStation()
        {
            if (_movingTowardStation)
            {
                if (Vector3.Distance(transform.position, _targetPos.position) >=
                    healingDistance) return;
            
                _agent.SetDestination(transform.position);
                _movingTowardStation = false;
            
                // TODO: randomly moves inside the healing station;
            }
            else
            {
                _targetPos = _healingStationSensor.Sense().transform;
                // Check if there is a nearest weapon
                if (_targetPos == null) return;
                
                
                // Move towards the nearest weapon
                _agent.SetDestination(_targetPos.position);
                Debug.Log("Moving to nearest heal station");
                _movingTowardStation = true;
            }
        }
    }
}
