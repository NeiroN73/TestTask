using System;
using System.Threading;
using Content.Scripts.EventBus;
using Cysharp.Threading.Tasks;
using FishNet.Connection;
using Game.Creatures;
using GameCore.Factories;
using UnityEngine;
using VContainer;

namespace Content.Scripts.Factories
{
    public class NetworkBehavioursClientFactory : Factory
    {
        [Inject] private NetworkEventBus _networkEventBus;
        
        public async UniTask<T> RequestCreateAsync<T>(
            string id, 
            NetworkConnection networkConnection,
            Vector3 position = default, 
            Quaternion rotation = default, 
            Transform parent = null,
            CancellationToken cancellationToken = default) 
            where T : BaseNetworkBehaviour
        {
            var spawnArgs = new BehaviourSpawnClientRequestBroadcast
            {
                Id = id,
                NetworkConnection = networkConnection,
                Position = position,
                Rotation = rotation,
                Parent = parent
            };
            
            var completionSource = new UniTaskCompletionSource<T>();
            
            _networkEventBus.SubscribeOnClients<BehaviourSpawnServerResponseBroadcast>(
                (response, channel) =>
                {
                    if (response.SpawnedObject && response.SpawnedObject is T spawnedObject)
                    {
                        completionSource.TrySetResult(spawnedObject);
                    }
                    else
                    {
                        completionSource.TrySetException(
                            new InvalidCastException($"Cannot cast spawned object to {typeof(T).Name}"));
                    }
                });
            
            _networkEventBus.PublishServerRpc(spawnArgs);
            
            var result = await completionSource.Task.WithCancellation(cancellationToken);
            
            return result;
        }
    }
}