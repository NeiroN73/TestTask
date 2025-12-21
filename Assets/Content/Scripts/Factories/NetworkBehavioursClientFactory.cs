using System;
using System.Threading;
using Content.Scripts.EventBus;
using Cysharp.Threading.Tasks;
using FishNet.Connection;
using Game.Creatures;
using Game.LifetimeScopes;
using UnityEngine;
using VContainer;

namespace Content.Scripts.Factories
{
    public class NetworkBehavioursClientFactory : NetworkFactory, IClientInjectable
    {
        [Inject] private ClientEventBus _clientEventBus;
        [Inject] private ClientLifetimeScope _clientLifetimeScope;
        
        private UniTaskCompletionSource<BaseNetworkBehaviour> _completionSource;
        
        public async UniTask<T> RequestCreateAsync<T>(
            string id, 
            NetworkConnection networkConnection,
            Vector3 position = default, 
            Quaternion rotation = default, 
            Transform parent = null,
            CancellationToken cancellationToken = default) 
            where T : BaseNetworkBehaviour
        {
            _completionSource = new();
            
            _clientEventBus.ClientSubscribe<BehaviourSpawnServerResponseBroadcast>(OnBehaviourSpawnServerResponsedBroadcast).AddDisposable(Disposable);
            _clientEventBus.PublishServerRpc(new BehaviourSpawnClientRequestBroadcast
            {
                Id = id,
                NetworkConnection = networkConnection,
                Position = position,
                Rotation = rotation,
                Parent = parent
            });
            
            var result = await _completionSource.Task.WithCancellation(cancellationToken);
            
            if (result is T typedResult)
            {
                return typedResult;
            }
            else
            {
                throw new InvalidCastException($"Cannot cast spawned object to {typeof(T).Name}");
            }
        }
        
        private void OnBehaviourSpawnServerResponsedBroadcast(BehaviourSpawnServerResponseBroadcast broadcast)
        {
            if (broadcast.SpawnedObject)
            {
                _completionSource.TrySetResult(broadcast.SpawnedObject.GetComponent<BaseNetworkBehaviour>());
            }
            else
            {
                _completionSource.TrySetException(
                    new InvalidCastException($"Cannot cast spawned object to BaseNetworkBehaviour"));
            }
        }
    }
}