using System;
using System.Collections.Generic;
using FishNet;
using FishNet.Broadcast;
using FishNet.Connection;
using FishNet.Transporting;
using GameCore.Services;

namespace Content.Scripts.EventBus
{
    public sealed class NetworkEventBus : Service
    {
        private readonly Dictionary<Type, HashSet<object>> _subscriptionsByType = new();

        public void SubscribeOnServer<T>(Action<NetworkConnection, T, Channel> action) where T : struct, IBroadcast
        {
            var type = typeof(T);
            
            if (!_subscriptionsByType.TryGetValue(type, out var subscriptions))
            {
                subscriptions = new HashSet<object>();
                _subscriptionsByType[type] = subscriptions;
                InstanceFinder.ServerManager.RegisterBroadcast(action);
            }
            else
            {
                subscriptions.Add(action);
            }
        }
        
        public void SubscribeOnClients<T>(Action<T, Channel> action) where T : struct, IBroadcast
        {
            var type = typeof(T);
            
            if (!_subscriptionsByType.TryGetValue(type, out var subscriptions))
            {
                subscriptions = new HashSet<object>();
                _subscriptionsByType[type] = subscriptions;
                InstanceFinder.ClientManager.RegisterBroadcast(action);
            }
            else
            {
                subscriptions.Add(action);
            }
        }

        public void UnsubscribeOnServer<T>(Action<NetworkConnection, T, Channel> action) where T : struct, IBroadcast
        {
            var type = typeof(T);
            if (_subscriptionsByType.TryGetValue(type, out var subscriptions))
            {
                subscriptions.Remove(action);
                if (subscriptions.Count == 0)
                {
                    InstanceFinder.ServerManager.UnregisterBroadcast(action);
                    _subscriptionsByType.Remove(type);
                }
            }
        }
        
        public void UnsubscribeOnClients<T>(Action<T, Channel> action) where T : struct, IBroadcast
        {
            var type = typeof(T);
            if (_subscriptionsByType.TryGetValue(type, out var subscriptions))
            {
                subscriptions.Remove(action);
                if (subscriptions.Count == 0)
                {
                    InstanceFinder.ClientManager.UnregisterBroadcast(action);
                    _subscriptionsByType.Remove(type);
                }
            }
        }

        public void PublishServerRpc<T>(T message, Channel channel = Channel.Reliable) where T : struct, IBroadcast
        {
            if (_subscriptionsByType.ContainsKey(typeof(T)))
            {
                InstanceFinder.ClientManager.Broadcast(message, channel: channel);
            }
        }

        public void PublishClientsRpc<T>(T message, Channel channel = Channel.Reliable) where T : struct, IBroadcast
        {
            if (_subscriptionsByType.ContainsKey(typeof(T)))
            {
                InstanceFinder.ServerManager.Broadcast(message, channel: channel);
            }
        }

        public void PublishTargetRpc<T>(NetworkConnection connection, T message, Channel channel = Channel.Reliable)
            where T : struct, IBroadcast
        {
            if (_subscriptionsByType.ContainsKey(typeof(T)))
            {
                InstanceFinder.ServerManager.Broadcast(connection, message, channel: channel);
            }
        }
    }
}