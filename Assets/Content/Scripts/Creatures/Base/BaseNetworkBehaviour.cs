using FishNet.Connection;
using FishNet.Object;
using Game.Installers;
using Game.LifetimeScopes;
using GameCore.LifetimeScopes;
using VContainer.Unity;

namespace Game.Creatures
{
    public abstract class BaseNetworkBehaviour : NetworkBehaviour
    {
        private BaseLifetimeScope _lifetimeScope;
        
        protected ComponentsContainer ComponentsContainer;

        public void TryInitialize()
        {
            TryServerInitialize();
            TryClientInitialize();
        }
        
        public void TryServerInitialize()
        {
            _lifetimeScope = LifetimeScope.Find<ServerLifetimeScope>() as ServerLifetimeScope;
            ComponentsContainer = new(gameObject);
            Initialize();
        }
        
        [ObserversRpc]
        public void TryClientInitialize()
        {
            _lifetimeScope = LifetimeScope.Find<ClientLifetimeScope>() as ClientLifetimeScope;
            ComponentsContainer = new(gameObject);
            Initialize();
        }
        
        protected virtual void Initialize()
        {
        }
        
        protected void InitializeComponents()
        {
            var components = ComponentsContainer.Components;
            foreach (var component in components)
            {
                component.Configure(ComponentsContainer);
            }
            
            NetworkObjectInitializeUtils.InitializeNetworkObjects(components, IsOwner, _lifetimeScope);
        }
    }
}