using System;
using UnityEngine;

namespace Final_Project.Robot_Types.Prefab
{
    public class WeaponPickup : MonoBehaviour
    {
        private string _weaponName;

        private void Start()
        {
            _weaponName = gameObject.name.Contains("Ammo Pickup Machine Gun") ? "Machine Gun" :
                gameObject.name.Contains("Ammo Pickup Sniper") ? "Sniper" :
                gameObject.name.Contains("Ammo Pickup Rocket Launcher") ? "Rocket Launcher" :
                gameObject.name.Contains("Ammo Pickup Shotgun") ? "Shotgun" : "";
        }

        private void OnTriggerEnter(Collider other)
        {
            // Check if the collider that entered the trigger is the player
            if (other.CompareTag("Player") || other.CompareTag("Enemy"))
            {
                // Activate the weapon on the player
                Transform weaponTransform = other.transform.Find("Visuals").Find("Weapons").Find(_weaponName);
                if (weaponTransform != null)
                {
                    Debug.Log("Setting active the weapon");
                    weaponTransform.gameObject.SetActive(true);
                }
                // Destroy the pickup game object
                Destroy(gameObject);
            }
        }
    }
}
