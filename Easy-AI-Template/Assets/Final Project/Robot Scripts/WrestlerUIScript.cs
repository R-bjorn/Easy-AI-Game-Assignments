using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Final_Project.Robot_Scripts
{
    public class WrestlerUIScript : MonoBehaviour
    {
        // public GameObject player;
        // public RandomAIScript randomAI;
        // public MovingToHealthLocationScript movingToHealthLocation;
        // public AttackingAIScript attackingAI;
        //
        // public void OnRadioButtonSelected(int index)
        // {
        //     switch (index)
        //     {
        //         case 0:
        //             randomAI.enabled = true;
        //             movingToHealthLocation.enabled = false;
        //             attackingAI.enabled = false;
        //             break;
        //         case 1:
        //             randomAI.enabled = false;
        //             movingToHealthLocation.enabled = true;
        //             attackingAI.enabled = false;
        //             break;
        //         case 2:
        //             randomAI.enabled = false;
        //             movingToHealthLocation.enabled = false;
        //             attackingAI.enabled = true;
        //             break;
        //     }
        // }
    
        private ToggleGroup _toggleGroup;

        private void Start()
        {
            _toggleGroup = GetComponent<ToggleGroup>();
        }

        public void Submit()
        {
            Toggle toggle = _toggleGroup.ActiveToggles().FirstOrDefault();
            if (toggle is not null) Debug.Log(toggle.name + " _ " + toggle.GetComponentInChildren<Text>().text);
        }
    }
}
