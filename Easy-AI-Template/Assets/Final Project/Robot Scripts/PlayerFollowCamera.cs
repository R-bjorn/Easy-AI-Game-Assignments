using Cinemachine;
using UnityEngine;

namespace Final_Project.Robot_Scripts
{
    public class PlayerFollowCamera : Singleton<PlayerFollowCamera>
    {
        private CinemachineVirtualCamera _cinemachineVirtualCamera;

        private void Awake()
        {
            _cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        }

        public void FollowPlayer(Transform playerTransform)
        {
            _cinemachineVirtualCamera.Follow = playerTransform;
        }
    }
}
