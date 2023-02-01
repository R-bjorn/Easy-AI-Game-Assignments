using UnityEngine;

namespace EasyAI.Cameras
{
    /// <summary>
    /// Camera for looking at an agent from a set position.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Camera))]
    public class LookAtAgentCamera : MonoBehaviour
    {
        [Tooltip("How much to vertically offset the camera for viewing agents.")]
        [Min(0)]
        [SerializeField]
        private float offset = 1;

        [Tooltip("How fast the camera should look to the agent for smooth looking. Set to zero for instant camera looking.")]
        [Min(0)]
        [SerializeField]
        private float lookSpeed = 5;

        private void Start()
        {
            // Snap look right away.
            float look = lookSpeed;
            lookSpeed = 0;
            LateUpdate();
            lookSpeed = look;
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
                else
                {
                    return;
                }
            }
            
            // Determine where to look at.
            Vector3 target = agent.Visuals == null ? agent.transform.position : agent.Visuals.position;
            target = new(target.x, target.y + offset, target.z);

            // Look instantly.
            if (lookSpeed <= 0)
            {
                transform.LookAt(target);
                return;
            }

            // Look towards it.
            Transform t = transform;
            transform.rotation = Quaternion.Slerp(t.rotation, Quaternion.LookRotation(target - t.position), lookSpeed * Time.unscaledDeltaTime);
        }
    }
}