using System;
using Content.Scripts.EventBus;

namespace Game.Components
{
    public abstract class NetworkElement : IDisposable
    {
        protected readonly NetworkEventDisposable Disposable = new();
        
        public void Dispose()
        {
            Disposable.Dispose();
        }
        
        public virtual void InvokeSubscribes() {}
        public virtual void InvokePublishes() {}
    }

    public abstract class ServerNetworkElement : NetworkElement
    {
        protected ServerEventBus BehaviourEventBus;
        
        public void Initialize(ServerEventBus serverBehaviourEventBus)
        {
            BehaviourEventBus = serverBehaviourEventBus;
        }
    }

    public abstract class ClientNetworkElement : NetworkElement
    {
        protected ClientEventBus BehaviourEventBus;
        
        public void Initialize(ClientEventBus clientBehaviourEventBus)
        {
            BehaviourEventBus = clientBehaviourEventBus;
        }
    }
}