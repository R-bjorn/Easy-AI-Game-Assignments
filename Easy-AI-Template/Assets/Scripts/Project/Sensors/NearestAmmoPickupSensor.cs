using EasyAI;
using Project.Pickups;
using UnityEngine;

namespace Project.Sensors
{
    /// <summary>
    /// Sensor to sense the nearest ammo pickup to a soldier.
    /// </summary>
    [DisallowMultipleComponent]
    public class NearestAmmoPickupSensor : Sensor
    {
        /// <summary>
        /// Sense the nearest ammo pickup to a soldier.
        /// </summary>
        /// <returns>The nearest available ammo pickup, prioritizing the soldier's weapon priority, or null if no pickups available.</returns>
        public override object Sense()
        {
            if (Agent is not Soldier soldier)
            {
                return null;
            }
            
            // Store the chosen ammo pickup to move to.
            HealthAmmoPickup selected = null;
            int priority = int.MaxValue;
            
            // Go through every weapon type to consider pickups.
            for (int i = 0; i < soldier.WeaponPriority.Length; i++)
            {
                // If the weapon has infinite ammo or has its max ammo, continue.
                if (soldier.Weapons[i].MaxAmmo < 0 || soldier.Weapons[i].Ammo >= soldier.Weapons[i].MaxAmmo)
                {
                    continue;
                }

                // If a pickup has already been found and the priority of that weapon is higher, continue.
                if (selected != null && priority <= soldier.WeaponPriority[i])
                {
                    continue;
                }
                
                // Get the nearest available ammo pickup.
                HealthAmmoPickup pickup = SoldierManager.NearestAmmoPickup(soldier, i);

                // If no ammo pickup was available, continue.
                if (pickup == null)
                {
                    continue;
                }

                // Set the chosen pickup.
                selected = pickup;
                priority = soldier.WeaponPriority[i];
            }

            return selected;
        }
    }
}