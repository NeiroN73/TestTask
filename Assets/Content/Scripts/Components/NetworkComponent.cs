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

        public override void OnStartClient() => SubscribeSyncVars();
        public override void OnStopClient() => UnsubscribeSyncVars();

        public virtual void SubscribeSyncVars() {}
        public virtual void UnsubscribeSyncVars() {}
        
        public void Dispose()
        {
            Disposable.Dispose();
        }
    }
}