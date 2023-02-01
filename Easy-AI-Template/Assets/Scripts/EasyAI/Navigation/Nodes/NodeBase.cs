using System.Linq;
using UnityEngine;

namespace EasyAI.Navigation.Nodes
{
    /// <summary>
    /// Base class for node creation components.
    /// </summary>
    public abstract class NodeBase : MonoBehaviour
    {
        /// <summary>
        /// Destroy the component and its game object if it is the only component on it.
        /// </summary>
        public void Finish()
        {
            enabled = false;
            if (transform.childCount == 0 && !GetComponents<MonoBehaviour>().Any(m => m != this && m.enabled))
            {
                Destroy(gameObject);
                return;
            }

            Destroy(this);
        }
    }
}