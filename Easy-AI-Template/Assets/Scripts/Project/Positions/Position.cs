using UnityEngine;

namespace Project.Positions
{
    /// <summary>
    /// Base class for points that soldiers can interact with.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(BoxCollider))]
    public abstract class Position : MonoBehaviour
    {
        /// <summary>
        /// If this is for the red team.
        /// </summary>
        public bool RedTeam => redTeam;
        
        [Tooltip("If this is for the red team.")]
        [SerializeField]
        private bool redTeam = true;
        
        /// <summary>
        /// How many soldiers are within this point.
        /// </summary>
        protected int Count;
        
        private void Start()
        {
            // Ensure it is a trigger.
            GetComponent<BoxCollider>().isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            ++Count;
        }

        private void OnTriggerExit(Collider other)
        {
            --Count;
        }
    }
}