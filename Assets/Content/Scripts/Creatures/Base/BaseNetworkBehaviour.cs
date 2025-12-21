using System.Collections.Generic;
using Content.Scripts.EventBus;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using Game.Components;
using Game.Installers;
using Game.LifetimeScopes;
using R3;
using VContainer.Unity;

namespace Game.Creatures
{
    public abstract class BaseNetworkBehaviour : NetworkBehaviour
    {
        private ServerLifetimeScope _serverLifetimeScope;
        private ClientLifetimeScope _clientLifetimeScope;
        
        private List<NetworkElement> _elements = new();
        
        private readonly bool _isServerInitialized;
        private readonly bool _isClientInitialized;
        
        protected CompositeDisposable Disposable = new();
        
        protected ServerEventBus ServerBehaviourEventBus;
        protected ClientEventBus ClientBehaviourEventBus;

        public void TryInitialize()
        {
            TryServerInitialize();
            TryClientInitialize();
        }
        
        public void TryServerInitialize()
        {
            _serverLifetimeScope = LifetimeScope.Find<ServerLifetimeScope>() as ServerLifetimeScope;
            ServerBehaviourEventBus = new();
            ServerInitialize();
        }
        
        [ObserversRpc]
        public void TryClientInitialize()
        {
            _clientLifetimeScope = LifetimeScope.Find<ClientLifetimeScope>() as ClientLifetimeScope;
            ClientBehaviourEventBus = new();
            ClientInitialize();
        }

        protected virtual void ServerInitialize()
        {
        }
        
        protected virtual void ClientInitialize()
        {
        }
        
        protected void ServerInitializeElements(params ServerNetworkElement[] elements)
        {
            _elements.AddRange(elements);
            
            foreach (var element in elements)
            {
                element.Initialize(ServerBehaviourEventBus);
                element.InvokeSubscribes();
                element.InvokePublishes();
            }
            
            NetworkObjectInitializeUtils.InitializeServerObjects(_elements, _serverLifetimeScope);
        }
        
        protected void ClientInitializeElements(params ClientNetworkElement[] elements)
        {
            _elements.AddRange(elements);
            
            foreach (var element in elements)
            {
                element.Initialize(ClientBehaviourEventBus);
                element.InvokeSubscribes();
                element.InvokePublishes();
            }
            
            NetworkObjectInitializeUtils.InitializeClientObjects(_elements, _clientLifetimeScope, IsController);
        }
    }
}