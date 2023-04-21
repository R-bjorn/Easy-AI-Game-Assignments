using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Final_Project.Menu
{
    public class MainMenu : MonoBehaviour
    {

        [SerializeField] private Button startGame;
        [SerializeField] private Button controls;
        [SerializeField] private Button muteBtn;
        [SerializeField] private Button exitBtn;
        [SerializeField] private Button backBtn;

        [SerializeField] private GameObject controlsMenu;
        [SerializeField] private GameObject mainMenu;

        private bool _muted;
    
        // Start is called before the first frame update
        private void Start()
        {
            startGame.onClick.AddListener(() =>
            {
                SceneManager.LoadScene(1);
            });
        
            controls.onClick.AddListener(() =>
            {
                mainMenu.SetActive(false);
                controlsMenu.SetActive(true);
            });
        
            muteBtn.onClick.AddListener(() =>
            {
                _muted = !_muted;

                AudioListener.volume = (_muted) ? 0 : 1;
            });
        
            exitBtn.onClick.AddListener(Application.Quit);
            
            backBtn.onClick.AddListener(() =>
            {
                mainMenu.SetActive(true);
                controlsMenu.SetActive(false);
            });
        }
    }
}
