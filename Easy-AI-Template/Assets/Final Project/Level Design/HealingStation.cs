using UnityEngine;

namespace Final_Project.Level_Design
{
    public class HealingStation : MonoBehaviour
    {
        // The amount of health to restore to the player when triggered
        public float healAmount = 10f;

        // The tag of the player GameObject
        public string playerTag = "Player";

        // Called when another collider enters this trigger collider
        private void OnTriggerEnter(Collider other)
        {
            // Check if the other collider belongs to the player
            if (!other.gameObject.CompareTag(playerTag)) return;
            // Get the PlayerHealth component on the player GameObject
            Robot playerHealth = other.gameObject.GetComponent<Robot>();

            // Check if the player has a PlayerHealth component
            if (playerHealth != null)
            {
                // Call the PlayerHealth.Heal() method to restore the player's health
                playerHealth.Heal(healAmount);
            }
        }
    }
}
