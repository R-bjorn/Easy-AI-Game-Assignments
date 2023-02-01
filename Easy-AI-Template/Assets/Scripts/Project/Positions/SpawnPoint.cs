using System.Collections;
using UnityEngine;

namespace Project.Positions
{
    /// <summary>
    /// Positions for soldiers to spawn at.
    /// </summary>
    [DisallowMultipleComponent]
    public class SpawnPoint : Position
    {
        /// <summary>
        /// If this point has recently been used.
        /// </summary>
        public bool Used { get; set; }

        /// <summary>
        /// If this position is free to spawn at.
        /// </summary>
        public bool Open => !Used && Count == 0;

        /// <summary>
        /// Use this spawn point to block spawns at it.
        /// </summary>
        public void Use()
        {
            StopAllCoroutines();
            StartCoroutine(UseDelay());
        }

        /// <summary>
        /// Delay the spawn point from being used.
        /// </summary>
        /// <returns></returns>
        private IEnumerator UseDelay()
        {
            Used = true;
            yield return new WaitForSeconds(1);
            Used = false;
        }
    }
}