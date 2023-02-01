using UnityEngine;

namespace A2.Pickups
{
    /// <summary>
    /// Pickup to make a microbe able to mate again.
    /// </summary>
    [DisallowMultipleComponent]
    public class MatePickup : MicrobeBasePickup
    {
        /// <summary>
        /// The behaviour of the pickup.
        /// </summary>
        /// <param name="microbe">The microbe which picked up this pickup.</param>
        protected override void Execute(Microbe microbe)
        {
            microbe.Log("Powered up -  can now mate again!");
            microbe.CanMate();
        }
    }
}