using System.Collections.Generic;
using FishNet.Object;
using Game.Components;
using Game.Installers;
using Game.LifetimeScopes;
using R3;

namespace Game.Creatures
{
    public abstract class BaseNetworkBehaviour : NetworkBehaviour
    {
        private ServerLifetimeScope _serverLifetimeScope;
        private ClientLifetimeScope _clientLifetimeScope;
        
        private List<NetworkElement> _elements = new();
        
        protected CompositeDisposable Disposable = new();
        
        public void TryServerInitialize(ServerLifetimeScope scope)
        {
            _serverLifetimeScope = scope;
            ServerInitialize();
        }
        
        public void TryClientInitialize(ClientLifetimeScope scope)
        {
            _clientLifetimeScope = scope;
            ClientInitialize();
        }
        
        protected virtual void ServerInitialize() {}
        protected virtual void ClientInitialize() {}
        
        protected void ServerInitializeElements(params NetworkElement[] elements)
        {
            _elements.AddRange(elements);
            
            NetworkObjectInitializeUtils.InitializeServerObjects(_elements, _serverLifetimeScope);
        }
        
        protected void ClientInitializeElements(params NetworkElement[] elements)
        {
            _elements.AddRange(elements);
            
            NetworkObjectInitializeUtils.InitializeClientObjects(_elements, _clientLifetimeScope, IsController);
        }
    }
}