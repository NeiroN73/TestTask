using System;
using System.Collections.Generic;
using System.Threading;
using FishNet.Object;
using FishNet.Connection;
using R3;
using UnityEngine;

namespace Content.Scripts.EventBus
{
    public sealed class NetworkEventBus : NetworkBehaviour, IDisposable
    {
        private readonly Dictionary<long, object> _subjects = new();

        // Type-safe подписка
        public Observable<T> Subscribe<T>() where T : struct, INetworkEvent
        {
            var id = NetworkEventId<T>.Id;

            if (!_subjects.TryGetValue(id, out var subject))
            {
                subject = new Subject<T>();
                _subjects.Add(id, subject);
            }

            return (Subject<T>)subject;
        }

        // Локальная публикация
        public void Publish<T>(T evt) where T : struct, INetworkEvent
        {
            var id = NetworkEventId<T>.Id;
            if (_subjects.TryGetValue(id, out var subject))
                ((Subject<T>)subject).OnNext(evt);
        }

        // ----------------------------
        // Конкретные RPC под PlayerMovedEvent
        // ----------------------------

        [ServerRpc(RequireOwnership = false)]
        public void PublishServerPlayerMoved(PlayerMovedEvent evt)
        {
            PublishObserversPlayerMoved(evt);
        }

        [ObserversRpc]
        private void PublishObserversPlayerMoved(PlayerMovedEvent evt)
        {
            Publish(evt);
        }

        [TargetRpc]
        private void PublishTargetPlayerMoved(NetworkConnection target, PlayerMovedEvent evt)
        {
            Publish(evt);
        }

        [ServerRpc(RequireOwnership = false)]
        public void PublishTargetFromServerPlayerMoved(PlayerMovedEvent evt, NetworkConnection target)
        {
            PublishTargetPlayerMoved(target, evt);
        }

        // ----------------------------

        public override void OnStopNetwork()
        {
            Dispose();
            base.OnStopNetwork();
        }

        public void Dispose()
        {
            foreach (var s in _subjects.Values)
                if (s is IDisposable d)
                    d.Dispose();

            _subjects.Clear();
        }
    }

    public interface INetworkEvent { }

    public struct PlayerMovedEvent : INetworkEvent
    {
        public Vector3 Position;

        public PlayerMovedEvent(Vector3 position)
        {
            Position = position;
        }
    }

    internal static class NetworkEventId<T>
    {
        private static long _nextId;
        public static readonly long Id = Interlocked.Increment(ref _nextId);
    }

    public static class NetworkEventBusExtensions
    {
        public static void SendToServerPlayerMoved(this NetworkEventBus bus, PlayerMovedEvent evt)
        {
            bus.PublishServerPlayerMoved(evt);
        }

        public static void SendToClientPlayerMoved(this NetworkEventBus bus, PlayerMovedEvent evt, NetworkConnection target)
        {
            bus.PublishTargetFromServerPlayerMoved(evt, target);
        }
    }
}
