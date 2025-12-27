using FishNet.Object;
using FishNet.Object.Synchronizing;
using Game.NetworkInterfaces;
using Game.Observers;
using Game.Services;
using TMPro;
using VContainer;

namespace Game.Components
{
    public class ChangeNameComponent : NetworkComponent, IInjectable, IClientPreInitializable,
        ILocalClientInitializable, IClientDisposable
    {
        private readonly SyncVar<string> _userName = new();
        
        private TMP_Text _userNameText;
        
        [Inject] private PlayerState _playerState;

        public Observer<string> UsernameChanged = new();

        public void Configure(TMP_Text userNameText)
        {
            _userNameText = userNameText;
        }
        
        public void ClientPreInitialize()
        {
            _userName.OnChange += OnUsernameChanged;
        }
        
        public void LocalClientInitialize()
        {
            SetUsernameServerRpc(_playerState.Username);
        }
        
        private void OnUsernameChanged(string prevValue, string nextValue, bool asServer)
        {
            UpdateNameText(nextValue);
        }

        private void UpdateNameText(string name)
        {
            if (_userNameText)
            {
                _userNameText.text = name;
                if (IsOwner)
                {
                    UsernameChanged.Publish(name);
                }
            }
        }

        [ServerRpc]
        private void SetUsernameServerRpc(string username)
        {
            _userName.Value = username;
        }

        public void ClientDispose()
        {
            _userName.OnChange -= OnUsernameChanged;
        }
    }
}