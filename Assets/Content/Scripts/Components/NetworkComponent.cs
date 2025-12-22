using System;
using FishNet.Object;
using Game.Creatures;
using R3;

namespace Game.Components
{
    public abstract class NetworkComponent : NetworkBehaviour, IDisposable
    {
        protected readonly CompositeDisposable Disposable = new();
        
        protected ComponentsContainer ComponentsContainer;

        public void Configure(ComponentsContainer componentsContainer)
        {
            ComponentsContainer = componentsContainer;
        }
        
        public void Dispose()
        {
            Disposable.Dispose();
        }
    }

    public abstract class ServerNetworkComponent : NetworkComponent
    {
    }

    public abstract class ClientNetworkComponent : NetworkComponent
    {
    }
}