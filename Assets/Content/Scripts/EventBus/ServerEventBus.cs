using System;
using System.Collections.Generic;
using FishNet;
using FishNet.Broadcast;
using FishNet.Connection;
using FishNet.Transporting;

namespace Content.Scripts.EventBus
{
    public sealed class ServerEventBus : NetworkEventBus
    {
        public NetworkEventHandler ServerSubscribe<T>(Action<T> action) where T : struct, IBroadcast
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
            
            void Handler(NetworkConnection c, T t, Channel ch) => InvokeSubscribes(t);
            InstanceFinder.ServerManager.RegisterBroadcast((Action<NetworkConnection, T, Channel>)Handler, false);
            
            return new NetworkEventHandler(action, this, type);
        }
        
        public void PublishClientsRpc<T>(T message, Channel channel = Channel.Reliable) where T : struct, IBroadcast
        {
            InstanceFinder.ServerManager.Broadcast(message, requireAuthenticated: false, channel: channel);
        }

        public void PublishTargetRpc<T>(NetworkConnection connection, T message, Channel channel = Channel.Reliable)
            where T : struct, IBroadcast
        {
            InstanceFinder.ServerManager.Broadcast(connection, message, requireAuthenticated: false, channel: channel);
        }
    }
}