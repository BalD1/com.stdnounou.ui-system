using UnityEngine;

namespace StdNounou.UI
{
    public class MainMenu : UIMainScreen
    {
        [Header("Main Menu specifics")]

        [Header("Buttons")]
        [SerializeField] private ButtonBase playButton;
        [SerializeField] private ButtonBase optionsButton;
        [SerializeField] private ButtonBase creditsButton;
        [SerializeField] private ButtonBase quitButton;

        [Header("Screens")]
        [SerializeField] private UIScreen optionsScreen;
        [SerializeField] private UIScreen creditsScreen;

        protected override void Awake()
        {
            base.Awake();

            optionsButton.AddListenerToOnClick(OpenOptionsScreen);
            creditsButton.AddListenerToOnClick(OpenCreditsScreen);
            quitButton.AddListenerToOnClick(() => Application.Quit());
        }

        private void OpenOptionsScreen()
        {
            optionsScreen.Open(true);
        }

        private void OpenCreditsScreen()
        {
            creditsScreen.Open(true);
        }
    } 
}
