using Final_Project.Extra_Scripts;
using Final_Project.Sensors;
using UnityEngine;
using UnityEngine.AI;

namespace Final_Project.Robot_Types
{
    public class AttackingRobotAI : MonoBehaviour
    {
        private Wrestler _wrestler;
        // Reference to the sensor
        private NearestWeaponSensor _weaponSensor;
        private NearestEnemySensor _enemySensor;
        
        // The distance at which the player can pick up the weapon
        public float pickupDistance = 0.2f;
        public float fightDistance = 3f;

        // Whether the player is currently moving towards the weapon
        private bool _movingTowardsWeapon;
        private bool _movingTowardsEnemy;
        
        // Weapon pickup target
        private Transform _targetWeapon;
        private Transform _targetEnemy;
        
        private NavMeshAgent _agent;
        
        private void Start()
        {
            _movingTowardsWeapon = false;
            _movingTowardsEnemy = false;
            _agent = GetComponent<NavMeshAgent>();

            _weaponSensor = _agent.GetComponent<NearestWeaponSensor>();
            _enemySensor = _agent.GetComponent<NearestEnemySensor>();

            _wrestler = _agent.GetComponent<Wrestler>();
            
            ChooseNewTargetPoint();
        }

        private void FixedUpdate()
        {
            if(!_wrestler.hasWeapon)
                if (!_movingTowardsWeapon)
                    ChooseNewTargetPoint();
                else
                    CheckingPlayerPosition();
            else
                if (!_movingTowardsEnemy)
                    ChooseNewTargetEnemy();
                else
                    ChaseEnemy();
        }

        private void ChooseNewTargetEnemy()
        {
            // Check if already has a target position
            if (_targetEnemy != null) return;

            // Get a new target position using Sensor
            _targetEnemy = _enemySensor.Sense().transform;
            if (_targetEnemy == null) return;

            // Move towards the nearest enemy
            _agent.isStopped = false;
            _agent.SetDestination(_targetEnemy.position);

            // Enable the moving towards enemy bool
            _movingTowardsEnemy = true;
        }

        private void ChaseEnemy()
        {
            // Check if the player has reached the weapon
            if (Vector3.Distance(transform.position, _targetEnemy.position) >= fightDistance) return;

            if (_agent.CompareTag("Player"))
            {
                Debug.Log("Found Enemy! Attacking....");
                // TODO: Fire the weapon
            }
        }

        private void CheckingPlayerPosition()
        {
            // Check if the player has reached the weapon
            if (Vector3.Distance(transform.position, _targetWeapon.position) >= pickupDistance) return;

            if (_agent.CompareTag("Player") && _agent.GetComponent<Wrestler>().hasWeapon)
            {
                // Stop the player from moving
                // _agent.SetDestination(transform.position);
                _agent.isStopped = true;
                _movingTowardsWeapon = false;
                _targetWeapon = null;
            }
        }
        
        private void ChooseNewTargetPoint()
        {
            // Check if already has a target position
            if (_targetWeapon != null) return;
            
            // Get a new target position using Sensor
            _targetWeapon = _weaponSensor.Sense().transform;
            // Check if there is a nearest weapon
            if (_targetWeapon == null) return;
                
            // Move towards the nearest weapon
            _agent.isStopped = false;
            _agent.SetDestination(_targetWeapon.position);
            
            // Enable the moving towards weapon bool
            _movingTowardsWeapon = true;
        }
    }
}
