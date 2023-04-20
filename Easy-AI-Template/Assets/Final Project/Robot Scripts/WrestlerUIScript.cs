using System.Linq;
using Final_Project.Robot_Types;
using UnityEngine;
using UnityEngine.UI;

namespace Final_Project.Robot_Scripts
{
    public class WrestlerUIScript : MonoBehaviour
    {
        public GameObject player;

        private RobotRoamingAI _roamingAI;
        private MedicRobotAI _medicAI;
        private AttackingRobotAI _attackAI;
        
        private ToggleGroup _toggleGroup;
        
        private void OnRadioButtonSelected(string index)
        {
            switch (index)
            {
                case "     Random AI Behaviour":
                    _medicAI.enabled = false;
                    _attackAI.enabled = false;
                    _roamingAI.enabled = true;
                    break;
                case "     Medic AI Behaviour":
                    _attackAI.enabled = false;
                    _roamingAI.enabled = false;
                    _medicAI.enabled = true;
                    break;
                case "     Attacking AI Behaviour":
                    _medicAI.enabled = false;
                    _roamingAI.enabled = false;
                    _attackAI.enabled = true;
                    break;
                default:
                    _medicAI.enabled = false;
                    _roamingAI.enabled = false;
                    _attackAI.enabled = false;
                    break;
            }
        }
    
        private void Start()
        {
            _toggleGroup = GetComponent<ToggleGroup>();
            
            _roamingAI = player.GetComponent<RobotRoamingAI>();
            _medicAI = player.GetComponent<MedicRobotAI>();
            _attackAI = player.GetComponent<AttackingRobotAI>();
            
        }

        public void Submit()
        {
            Toggle toggle = _toggleGroup.ActiveToggles().FirstOrDefault();

            if (toggle is null) return; 
            
            Debug.Log(toggle.name + " _ " + toggle.GetComponentInChildren<Text>().text);
            
            OnRadioButtonSelected(toggle.GetComponentInChildren<Text>().text);
        }
    }
}
