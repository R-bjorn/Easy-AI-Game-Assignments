using System;
using UnityEngine;

namespace A1.Creativity
{
    public class PickupObstacles : MonoBehaviour
    {
        private GameObject robotHand = null;

        public bool isNotPickedUp = true;

        private void Start()
        {
            robotHand = GameObject.FindGameObjectWithTag("Player");
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Player") && transform.gameObject.CompareTag("Pickups"))
                GrabObstacles();
        }

        public void GrabObstacles()
        {
            isNotPickedUp = false;
            if (robotHand != null)
            {
                transform.SetParent(robotHand.transform);
            }
           
        }
    }
}
