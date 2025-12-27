using System;
using FishNet;
using FishNet.Broadcast;
using FishNet.Connection;
using FishNet.Transporting;

namespace Game.NetworkEventBus
{
    public sealed class ServerEventBus : NetworkEventBus
    {
        public NetworkEventHandler ServerSubscribe<T>(Action<T> action) where T : struct, IBroadcast
        {
            UpdateSubscribes(action);
            
            void Handler(NetworkConnection c, T t, Channel ch) => InvokeSubscribes(t);
            InstanceFinder.ServerManager.RegisterBroadcast((Action<NetworkConnection, T, Channel>)Handler, false);
            
            return new NetworkEventHandler(action, this, typeof(T));
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