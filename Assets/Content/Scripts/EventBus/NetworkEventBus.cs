using System;
using System.Collections.Generic;
using FishNet.Broadcast;
using GameCore.Services;

namespace Content.Scripts.EventBus
{
    public abstract class NetworkEventBus : Service
    {
        protected readonly Dictionary<Type, List<Delegate>> SubscriptionsByType = new();

        public void Unsubscribe<T>(Delegate action) where T : struct, IBroadcast
        {
            Unsubscribe(action, typeof(T));
        }
        
        public void Unsubscribe(Delegate action, Type type)
        {
            if (SubscriptionsByType.TryGetValue(type, out var subscriptions))
            {
                subscriptions.Remove(action);
                if (subscriptions.Count == 0)
                {
                    SubscriptionsByType.Remove(type);
                }
            }
        }
        
        protected void InvokeSubscribes<T>(T message) where T : struct, IBroadcast
        {
            if (SubscriptionsByType.TryGetValue(typeof(T), out var subscriptions))
            {
                foreach (var subscription in subscriptions)
                {
                    ((Action<T>)subscription).Invoke(message);
                }
            }
        }
    }
}