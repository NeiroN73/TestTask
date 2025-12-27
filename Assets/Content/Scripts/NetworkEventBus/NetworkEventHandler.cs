using System;

namespace Game.NetworkEventBus
{
    public class NetworkEventHandler
    {
        public Type Type { get; }
        public Delegate Action { get; }
        public NetworkEventBus NetworkEventBus { get; }

        public NetworkEventHandler(Delegate action, NetworkEventBus networkEventBus, Type type)
        {
            Type = type;
            Action = action;
            NetworkEventBus = networkEventBus;
        }
        
        public void AddDisposable(NetworkEventDisposable disposable)
        {
            disposable.Add(this);
        }
    }
}