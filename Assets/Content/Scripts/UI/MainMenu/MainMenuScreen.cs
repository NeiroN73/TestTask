using GameCore.UI;
using UnityEngine;

namespace Game.UI.MainMenu
{
    public class MainMenuScreen : Screen<MainMenuViewModel>
    {
        [SerializeField] private InputFieldBinder _playerName;
        [SerializeField] private ButtonBinder _hostButton;
        [SerializeField] private ButtonBinder _joinButton;
        [SerializeField] private TextBinder _hostText;
        
        public override void Initialize()
        {
            ViewModel.HostButton = _hostButton.Clicked;
            ViewModel.JoinButton = _joinButton.Clicked;
            ViewModel.PlayerNameInputField = _playerName.TextChangeInputed;
            ViewModel.PlayerNameInputFieldTextChanged = _playerName.TextChanged;
            ViewModel.TestText = _hostText.TestTextChanged;
            
            Build(_playerName, _hostButton, _joinButton, _hostText);
        }
    }
}