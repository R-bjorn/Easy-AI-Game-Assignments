using UnityEngine;

namespace Final_Project
{
    public class CameraOrbit : MonoBehaviour
    {

        [SerializeField] private float rotationSpeed = 10;
        private void Update()
        {
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        }
    }
}