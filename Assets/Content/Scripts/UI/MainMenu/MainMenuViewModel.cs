using Game.NetworkManagers;
using Game.Services;
using GameCore.Factories;
using GameCore.Services;
using GameCore.UI;
using GameCore.Utils;
using R3;
using VContainer;

namespace Game.UI.MainMenu
{
    public class MainMenuViewModel : ViewModel
    {
        [Inject] private ScreensService _screensService;
        [Inject] private ScenesService _scenesService;
        [Inject] private CreaturesFactory creaturesFactory;
        [Inject] private GameNetworkManager _gameNetworkManager;
        [Inject] private ConnectionService connectionService;
        
        private readonly RefTypeViewModelBinder<ReactiveCommand<string>> _playerNameInputField = new("playerName");
        private readonly RefTypeViewModelBinder<ReactiveCommand> _hostButton = new("hostButton");
        private readonly RefTypeViewModelBinder<ReactiveCommand> _joinButton = new("joinButton");

        private string _playerName;
        
        public override void Initialize()
        {
            Bind(_playerNameInputField, _hostButton, _joinButton);

            _playerNameInputField.Value.Subscribe(OnPlayerNameChanged).AddTo(Disposable);
            _hostButton.Value.Subscribe(OnHostClicked).AddTo(Disposable);
            _joinButton.Value.Subscribe(OnJoinClicked).AddTo(Disposable);
        }

        private void OnPlayerNameChanged(string playerName)
        {
            _playerName = playerName;
        }
        
        private async void OnHostClicked()
        {
            await connectionService.HostGameAsync(OnSuccess);
        }
        
        private async void OnJoinClicked()
        {
            await connectionService.JoinGameAsync(OnSuccess);
        }

        private void OnSuccess()
        {
            //_screensService.Close();
        }
    }
}