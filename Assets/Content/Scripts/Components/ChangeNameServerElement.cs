using Content.Scripts.EventBus;
using FishNet.Broadcast;
using Game.Creatures;
using Game.Services;
using TMPro;
using VContainer;

namespace Game.Components
{
    public class ChangeNameServerElement : ServerNetworkElement, IServerInitializable, IServerInjectable
    {
        [Inject] private PlayerState _playerState;
        
        private string _userName;
        
        public void Initialize()
        {
            _userName = _playerState.Username;
        }

        public override void InvokePublishes()
        {
            BehaviourEventBus.PublishClientsRpc(new ShowNameOnClientsEvent{UserName = _userName});
        }
    }

    public class ChangeNameClientElement : ClientNetworkElement
    {
        private readonly TMP_Text _userNameText;
        private string _userName;

        public ChangeNameClientElement(TMP_Text userNameText)
        {
            _userNameText = userNameText;
        }

        public override void InvokeSubscribes()
        {
            BehaviourEventBus.ClientSubscribe<ShowNameOnClientsEvent>(OnUpdateNameView).AddDisposable(Disposable);
        }

        private void OnUpdateNameView(ShowNameOnClientsEvent e)
        {
            _userNameText.text = e.UserName;
        }
    }

    public struct ShowNameOnClientsEvent : IBroadcast
    {
        public string UserName;
    }
}