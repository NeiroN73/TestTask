using System;
using System.Collections.Generic;
using FishNet.Broadcast;

namespace Game.NetworkEventBus
{
    //для избежания сетевых атрибутов, которые доступны только в NetworkBehaviour
    public abstract class NetworkEventBus
    {
        protected readonly Dictionary<Type, List<Delegate>> SubscriptionsByType = new();

        public void Publish<T>(T message) where T : struct, IBroadcast
        {
            InvokeSubscribes(message);
        }

        public void Subscribe<T>(Action<T> action) where T : struct, IBroadcast
        {
            UpdateSubscribes(action);
        }
        
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

        protected void UpdateSubscribes<T>(Action<T> action) where T : struct, IBroadcast
        {
            var type = typeof(T);
            if (SubscriptionsByType.TryGetValue(type, out var subscriptions))
            {
                subscriptions.Add(action);
            }
            else
            {
                SubscriptionsByType[type] = new List<Delegate> { action };
            }
        }
    }
}