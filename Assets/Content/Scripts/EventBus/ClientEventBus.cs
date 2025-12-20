using System;
using System.Collections.Generic;
using FishNet;
using FishNet.Broadcast;
using FishNet.Transporting;

namespace Content.Scripts.EventBus
{
    public sealed class ClientEventBus : NetworkEventBus
    {
        public NetworkEventHandler ClientsSubscribe<T>(Action<T> action) where T : struct, IBroadcast
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
            
            void Handler(T t, Channel ch) => InvokeSubscribes(t);
            InstanceFinder.ClientManager.RegisterBroadcast((Action<T, Channel>)Handler);
            return new NetworkEventHandler(action, this, type);
        }
        
        public void PublishServerRpc<T>(T message, Channel channel = Channel.Reliable) where T : struct, IBroadcast
        {
            InstanceFinder.ClientManager.Broadcast(message, channel);
        }
    }
}