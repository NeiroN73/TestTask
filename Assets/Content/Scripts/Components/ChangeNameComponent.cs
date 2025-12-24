using FishNet.Object;
using FishNet.Object.Synchronizing;
using Game.Creatures;
using Game.Services;
using TMPro;
using VContainer;

namespace Game.Components
{
    public class ChangeNameComponent : ClientNetworkComponent, IInjectable, IClientInitializable,
        ILocalClientInitializable, IClientDisposable
    {
        private readonly SyncVar<string> _userName = new();
        
        [Inject] private PlayerState _playerState;
        
        private TMP_Text _userNameText;
        
        public void Configure(TMP_Text userNameText)
        {
            _userNameText = userNameText;
        }

        public void ClientInitialize()
        {
            _userName.OnChange += OnUsernameChanged;
        }
        
        public void LocalClientInitialize()
        {
            SetUsernameServerRpc(_playerState.Username);
        }

        private void OnUsernameChanged(string oldValue, string newValue, bool asServer)
        {
            _userNameText.text = newValue;
        }

        [ServerRpc]
        private void SetUsernameServerRpc(string username)
        {
            _userName.Value = username;
        }

        public void ClientDispose()
        {
            //_userName.OnChange -= OnUsernameChanged;
        }
    }
}