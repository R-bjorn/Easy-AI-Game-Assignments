using UnityEngine;

namespace Final_Project.Extra_Scripts
{
    public class Wrestler : MonoBehaviour
    {

        public int wrestlerHealth = 100;

        public bool hasWeapon;

        public string WeaponName => _weapon.name;

        private GameObject _weapon;
        
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void HoldWeapon(Transform weapon)
        {
            hasWeapon = true;
            _weapon = weapon.gameObject;
        }
    }
}
