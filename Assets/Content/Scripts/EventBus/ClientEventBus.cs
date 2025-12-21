using System;
using FishNet;
using FishNet.Broadcast;
using FishNet.Transporting;

namespace Content.Scripts.EventBus
{
    public sealed class ClientEventBus : NetworkEventBus
    {
        public NetworkEventHandler ClientSubscribe<T>(Action<T> action) where T : struct, IBroadcast
        {
            UpdateSubscribes(action);
            
            void Handler(T t, Channel ch) => InvokeSubscribes(t);
            InstanceFinder.ClientManager.RegisterBroadcast((Action<T, Channel>)Handler);
            return new NetworkEventHandler(action, this, typeof(T));
        }
        
        public void PublishServerRpc<T>(T message, Channel channel = Channel.Reliable) where T : struct, IBroadcast
        {
            InstanceFinder.ClientManager.Broadcast(message, channel);
        }
    }
}