using System.Collections;
using UnityEngine;

namespace A2
{
    /// <summary>
    /// Helper class to simply destroy an object after a given amount of time.
    /// </summary>
    [DisallowMultipleComponent]
    public class DestroyAfterTime : MonoBehaviour
    {
        [SerializeField]
        [Min(float.Epsilon)]
        [Tooltip("The time to wait before destroying this object")]
        private float duration = 1f;

        private void Start()
        {
            StartCoroutine(DestroyAfterSeconds());
        }

        /// <summary>
        /// Coroutine that waits for a given time before destroying this object.
        /// </summary>
        /// <returns>Nothing.</returns>
        private IEnumerator DestroyAfterSeconds()
        {
            yield return new WaitForSeconds(duration);
            Destroy(gameObject);
        }
    }
}