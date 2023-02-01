using UnityEngine;
using UnityEngine.InputSystem;

namespace EasyAI.Cameras
{
    /// <summary>
    /// Camera for tracking above an agent.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Camera))]
    public class TrackAgentCamera : MonoBehaviour
    {
        [Tooltip("How fast the camera should move to the agent for smooth movement. Set to zero for instant camera movement.")]
        [Min(0)]
        [SerializeField]
        private float moveSpeed = 5;

        [Tooltip("How high from the agent should the camera be.")]
        [Min(0)]
        [SerializeField]
        private float height = 10;

        [Tooltip("How low the camera can zoom in to.")]
        [Min(0)]
        [SerializeField]
        private float minHeight = 3;

        /// <summary>
        /// The attached camera.
        /// </summary>
        private Camera _camera;

        /// <summary>
        /// The target position to look at.
        /// </summary>
        private Vector3 _target;
        
        private void Start()
        {
            _camera = GetComponent<Camera>();
            
            // Snap look right away.
            float move = moveSpeed;
            moveSpeed = 0;
            LateUpdate();
            moveSpeed = move;
        }
        
        private void LateUpdate()
        {
            // Get the agent to look towards.
            Agent agent = Manager.CurrentlySelectedAgent;
            if (agent == null)
            {
                if (Manager.CurrentAgents.Count > 0)
                {
                    agent = Manager.CurrentAgents[0];
                }
            }

            if (agent != null)
            {
                _target = (agent.Visuals == null ? agent.transform : agent.Visuals).position;
            }

            // Allow for zooming in if this is the selected camera.
            if (Manager.SelectedCamera == _camera)
            {
                Vector2 scroll = Mouse.current.scroll.ReadValue();
                height -= scroll.y * Time.unscaledDeltaTime;
                height = Mathf.Max(height, minHeight);
            }

            // Move over the agent.
            Vector3 position = new(_target.x, _target.y + height, _target.z);
            transform.position = moveSpeed <= 0 ? position : Vector3.Slerp(transform.position, position, moveSpeed * Time.unscaledDeltaTime);
        }
    }
}