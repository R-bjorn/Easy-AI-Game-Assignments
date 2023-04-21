using Final_Project.Sensors;
using UnityEngine;
using UnityEngine.AI;

namespace Final_Project.Robot_Types
{
    public class AttackingRobotAI : MonoBehaviour
    {
        // Reference to the nearest weapon sensor
        private NearestWeaponSensor weaponSensor;
        
        // The distance at which the player can pick up the weapon
        public float pickupDistance = 0.2f;

        // Whether the player is currently moving towards the weapon
        private bool _movingTowardsWeapon;
        
        // Weapon pickup target
        private Transform _targetPos;
        
        private NavMeshAgent _agent;
        
        private void Start()
        {
            _agent = GetComponent<NavMeshAgent>();

            weaponSensor = _agent.GetComponent<NearestWeaponSensor>();
            ChooseNewTargetPoint();
        }

        private void FixedUpdate()
        {
            // ChooseNewTargetPoint();
        }
        
        private void ChooseNewTargetPoint()
        {
            // Check if the player is already moving towards the weapon
            if (_movingTowardsWeapon)
            {
                Debug.Log("Moving");
                // Check if the player has reached the weapon
                if (Vector3.Distance(transform.position, _targetPos.position) >=
                      pickupDistance) return;
                
                Debug.Log("Stopping the agent");
                // Stop the player from moving
                _agent.SetDestination(transform.position);
                _movingTowardsWeapon = false;

                // TODO: Pick up the weapon
                
            }
            else
            {
                _targetPos = weaponSensor.Sense().transform;
                // Check if there is a nearest weapon
                if (_targetPos == null) return;
                
                
                // Move towards the nearest weapon
                _agent.SetDestination(_targetPos.position);
                Debug.Log("Moving to new pickup location");
                _movingTowardsWeapon = true;
            }
        }
    }
}
