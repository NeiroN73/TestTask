using FishNet.Object;
using Game.NetworkInterfaces;
using R3;
using UnityEngine;

namespace Game.Components
{
    public class SendDebugComponent : NetworkComponent, IClientPreInitializable
    {
        private string _userName;
        
        public void ClientPreInitialize()
        {
            if (ComponentsContainer.TryGetNetworkComponent<ControllerComponent>(out var controllerComponent))
            {
                controllerComponent.DebugPerformed.Subscribe(OnDebugPerformedServerRpc).AddTo(Disposable);
            }
            if (ComponentsContainer.TryGetNetworkComponent<ChangeNameComponent>(out var changeNameComponent))
            {
                changeNameComponent.UsernameChanged.Subscribe(OnUsernameChangedServerRpc).AddTo(Disposable);
            }
        }
        
        [ServerRpc]
        private void OnDebugPerformedServerRpc()
        {
            DebugPerformObserversRpc(_userName);
        }

        [ObserversRpc]
        private void DebugPerformObserversRpc(string username)
        {
            DebugPerform(username);
        }
        
        [ServerRpc]
        private void OnUsernameChangedServerRpc(string userName)
        {
            _userName = userName;
        }

        private void DebugPerform(string username)
        {
            Debug.Log("Привет от " + username);
        }
    }
}