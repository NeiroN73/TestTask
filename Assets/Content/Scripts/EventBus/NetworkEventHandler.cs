using System;

namespace Content.Scripts.EventBus
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
    }
}