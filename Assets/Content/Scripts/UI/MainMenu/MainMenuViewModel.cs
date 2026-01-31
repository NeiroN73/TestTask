using Game.Services;
using GameCore.Factories;
using GameCore.ReactiveObservers;
using GameCore.Services;
using GameCore.UI;
using R3;
using VContainer;

namespace Game.UI.MainMenu
{
    public class MainMenuViewModel : ViewModel
    {
        [Inject] private ScreensService _screensService;
        [Inject] private ScenesService _scenesService;
        [Inject] private CreaturesFactory creaturesFactory;
        [Inject] private ConnectionService connectionService;
        [Inject] private PlayerState _playerState;

        public IReadOnlyReactiveObserver<string> PlayerNameInputField;
        public ReactiveObserver<string> PlayerNameInputFieldTextChanged;
        public IReadOnlyReactiveObserver HostButton;
        public IReadOnlyReactiveObserver JoinButton;
        public ReactiveObserver<string> TestText;
        
        public override void Initialize()
        {
            PlayerNameInputField.Subscribe(OnPlayerNameChanged).AddTo(Disposable);
            PlayerNameInputFieldTextChanged.Execute("newName");
            HostButton.Subscribe(OnHostClicked).AddTo(Disposable);
            JoinButton.Subscribe(OnJoinClicked).AddTo(Disposable);
            TestText.Value = "dsfsd";
        }

        private void OnPlayerNameChanged(string playerName)
        {
            _playerState.Username = playerName;
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
        }
    }
}