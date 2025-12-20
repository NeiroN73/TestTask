using System;
using System.Collections.Generic;
using Content.Scripts.EventBus;
using FishNet;
using FishNet.Broadcast;
using FishNet.Connection;
using FishNet.Transporting;
using Game.Creatures;
using Game.LifetimeScopes;
using GameCore.Configs;
using GameCore.Creatures;
using GameCore.Factories;
using GameCore.Services;
using UnityEngine;
using VContainer;
using Object = UnityEngine.Object;

namespace Content.Scripts.Factories
{
    public class NetworkBehavioursServerFactory : NetworkFactory, IServerInjectable, IServerInitializable
    {
        [Inject] private ServerLifetimeScope _serverLifetimeScope;
        [Inject] private BehavioursConfig _behavioursConfig;
        [Inject] private AssetsLoaderService _assetsLoaderService;
        [Inject] private ServerEventBus _serverEventBus;

        private Dictionary<string, BaseNetworkBehaviour> _behavioursById;

        public void Initialize()
        {
            _behavioursById = new();
        
            foreach (var handler in _behavioursConfig.Behaviours)
            {
                if (handler == null) continue;

                var behaviour = _assetsLoaderService.LoadAssetSync<BaseNetworkBehaviour>(handler.Asset); //TODO: сделать прелоадом
                
                if (!string.IsNullOrEmpty(handler.Id))
                    _behavioursById[handler.Id] = behaviour;
            }
            
            _serverEventBus.ServerSubscribe<BehaviourSpawnClientRequestBroadcast>(OnBehaviourSpawned).AddTo(Disposable);
        }
        
        private void OnBehaviourSpawned(BehaviourSpawnClientRequestBroadcast broadcast)
        {
            var behaviour = Create(broadcast.Id, broadcast.NetworkConnection,
                broadcast.Position, broadcast.Rotation, broadcast.Parent);
            
            var spawnedObjectArgs = new BehaviourSpawnServerResponseBroadcast
            {
                SpawnedObject = behaviour.gameObject
            };
            
            _serverEventBus.PublishTargetRpc(broadcast.NetworkConnection, spawnedObjectArgs);
        }
        
        public BaseNetworkBehaviour Create(string id, NetworkConnection conn, Vector3 position = default,
            Quaternion rotation = default, Transform parent = null)
        {
            var prefab = GetCreatureById(id);
            if (prefab == null)
                throw new InvalidOperationException($"Creature with ID '{id}' not found in config");

            var creature = Object.Instantiate(prefab, position, rotation, parent);
            InstanceFinder.ServerManager.Spawn(creature.gameObject, conn);
            InitializeBehaviour(creature);
            return creature;
        }
        
        private BaseNetworkBehaviour GetCreatureById(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
        
            if (_behavioursById.TryGetValue(id, out var creature))
            {
                return creature;
            }
            return null;
        }
        
        public void InitializeBehaviour<TBehaviour>(TBehaviour behaviour)
            where TBehaviour : BaseNetworkBehaviour
        {
            _serverLifetimeScope.Container.Inject(behaviour);
            behaviour.TryServerInitialize(_serverLifetimeScope);
        }
    }

    public struct BehaviourSpawnClientRequestBroadcast : IBroadcast
    {
        public string Id;
        public NetworkConnection NetworkConnection;
        public Vector3 Position;
        public Quaternion Rotation;
        public Transform Parent;
    }
    
    public struct BehaviourSpawnServerResponseBroadcast : IBroadcast
    {
        public GameObject SpawnedObject;
    }
}