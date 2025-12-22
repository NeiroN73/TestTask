using Game.Creatures;
using Game.Services;
using VContainer;

namespace Game.Components
{
    public class ChangeNameServerComponent : ServerNetworkComponent, IServerInjectable, IServerInitializable
    {
        [Inject] private PlayerState _playerState;
        
        private string _userName;

        public void Initialize()
        {
            _userName = _playerState.Username;
        }
    }
}