using UnityEngine;

namespace RepaintingBlocks
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        public UIMainMenu UIMainMenu;
        public UILanguage UILanguage;


        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            CloseAll();
            DisplayMainMenu(true);
        }

        public void CloseAll()
        {
            DisplayMainMenu(false);
            DisplayLanguageMenu(false);
        }

        public void DisplayMainMenu(bool isActive)
        {
            UIMainMenu.DisplayCanvas(isActive);
        }


        public void DisplayLanguageMenu(bool isActive)
        {
            UILanguage.DisplayCanvas(isActive);
        }
    }
}
