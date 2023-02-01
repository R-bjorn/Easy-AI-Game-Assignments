using UnityEngine;

namespace A2.Pickups
{
    /// <summary>
    /// Pickup to make a microbe not be hungry for a long time which for all purposes can be assumed to be the rest of the microbe's life.
    /// </summary>
    [DisallowMultipleComponent]
    public class NeverHungryPickup : MicrobeBasePickup
    {
        /// <summary>
        /// The behaviour of the pickup.
        /// </summary>
        /// <param name="microbe">The microbe which picked up this pickup.</param>
        protected override void Execute(Microbe microbe)
        {
            microbe.Log("Powered up - will not be hungry for eternity!");
            microbe.SetHunger(int.MinValue);
        }
    }
}