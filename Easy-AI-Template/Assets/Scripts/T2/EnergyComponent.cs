using UnityEngine;

namespace T2
{
    /// <summary>
    /// Allow for an object to have an energy level.
    /// </summary>
    [DisallowMultipleComponent]
    public class EnergyComponent : MonoBehaviour
    {
        /// <summary>
        /// The maximum energy level.
        /// </summary>
        public float MaxEnergy { get; private set; }
        
        /// <summary>
        /// Current energy.
        /// </summary>
        public float Energy => energy;
        
        [Tooltip("Current energy.")]
        [SerializeField]
        private float energy = 5;

        /// <summary>
        /// Restore energy.
        /// </summary>
        /// <param name="amount">The amount of energy to restore.</param>
        public void Replenish(float amount)
        {
            energy += amount;
            if (energy > MaxEnergy)
            {
                energy = MaxEnergy;
            }
        }

        /// <summary>
        /// Deplete energy.
        /// </summary>
        /// <param name="amount">The amount of energy to deplete.</param>
        public void Deplete(float amount)
        {
            energy -= amount;
            if (energy < 0)
            {
                energy = 0;
            }
        }

        private void Start()
        {
            MaxEnergy = energy;
        }
    }
}