using System.Collections;
using UnityEngine;

namespace Project.Pickups
{
    /// <summary>
    /// Pickup for health and ammo.
    /// </summary>
    [DisallowMultipleComponent]
    public class HealthAmmoPickup : PickupBase
    {
        /// <summary>
        /// How fast to spin its visuals in degrees per second.
        /// </summary>
        private const float Speed = 180;
        
        [Tooltip("Set to a negative number to be a health pickup, otherwise the weapon index of the player.")]
        [SerializeField]
        public int weaponIndex = -1;

        [Tooltip("The visuals object to rotate.")]
        [SerializeField]
        private Transform visuals;
        
        /// <summary>
        /// If the pickup is ready to be picked up.
        /// </summary>
        public bool Ready { get; set; } = true;
        
        /// <summary>
        /// All visuals of the pickup.
        /// </summary>
        private MeshRenderer[] _meshRenderers;
        
        /// <summary>
        /// Add health or ammo on pickup.
        /// </summary>
        /// <param name="soldier">The soldier.</param>
        /// <param name="ammo">The ammo array of the soldier.</param>
        protected override void OnPickedUp(Soldier soldier, int[] ammo)
        {
            // If not ready to be pickup up do nothing.
            if (!Ready)
            {
                return;
            }

            // If it was a health pickup, heal if the soldier is not at full health.
            if (weaponIndex < 0)
            {
                if (soldier.Health >= SoldierManager.Health)
                {
                    return;
                }
                
                soldier.Log("Picked up health.");
            
                soldier.Heal();
                StartCoroutine(ReadyDelay());

                return;
            }

            // Replenish ammo if needed.
            if (soldier.Weapons.Length <= weaponIndex || soldier.Weapons[weaponIndex].MaxAmmo < 0 || ammo[weaponIndex] >= soldier.Weapons[weaponIndex].MaxAmmo)
            {
                return;
            }
            
            soldier.Log((Soldier.WeaponIndexes) weaponIndex switch
            {
                Soldier.WeaponIndexes.MachineGun => "Replenished machine gun.",
                Soldier.WeaponIndexes.Shotgun => "Replenished shotgun.",
                Soldier.WeaponIndexes.Sniper => "Replenished sniper.",
                Soldier.WeaponIndexes.RocketLauncher => "Replenished rocket launcher.",
                _=> "Replenished pistol."
            });
            
            soldier.Weapons[weaponIndex].Replenish();
            StartCoroutine(ReadyDelay());
        }
        
        private void Start()
        {
            // Grab all meshes.
            _meshRenderers = GetComponentsInChildren<MeshRenderer>();
        }

        private void Update()
        {
            // Spin the visuals.
            visuals.Rotate(0, Speed * Time.deltaTime, 0, Space.Self);
        }

        /// <summary>
        /// Make the pickup not available for a given period of time.
        /// </summary>
        /// <returns>Nothing.</returns>
        private IEnumerator ReadyDelay()
        {
            Ready = false;
            ToggleMeshes();
            
            yield return new WaitForSeconds(SoldierManager.PickupTimer);
            
            Ready = true;
            ToggleMeshes();
        }

        /// <summary>
        /// Toggle all meshes on or off.
        /// </summary>
        private void ToggleMeshes()
        {
            foreach (MeshRenderer meshRenderer in _meshRenderers)
            {
                meshRenderer.enabled = Ready;
            }
        }
    }
}