using UnityEngine;

namespace A1.Creativity
{
    public class PickupObstacles : MonoBehaviour
    {
        private GameObject _robotHand;

        public bool isNotPickedUp = true;

        private void Start()
        {
            _robotHand = GameObject.FindGameObjectWithTag("Player");
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Player") && transform.gameObject.CompareTag("Pickups"))
                GrabObstacles();
        }

        public void GrabObstacles()
        {
            isNotPickedUp = false;
            if (_robotHand != null)
            {
                transform.SetParent(_robotHand.transform);
            }
           
        }
    }
}
