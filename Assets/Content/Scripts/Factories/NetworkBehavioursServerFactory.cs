using System;
using System.Collections.Generic;
using Content.Scripts.EventBus;
using FishNet;
using FishNet.Broadcast;
using FishNet.Connection;
using FishNet.Transporting;
using Game.Creatures;
using GameCore.Configs;
using GameCore.Creatures;
using GameCore.Factories;
using GameCore.Services;
using UnityEngine;
using VContainer;
using Object = UnityEngine.Object;

namespace Content.Scripts.Factories
{
    public class NetworkBehavioursServerFactory : Factory, IServerInjectable, IServerInitializable
    {
        [Inject] private IObjectResolver _objectResolver;
        [Inject] private BehavioursConfig behavioursConfig;
        [Inject] private AssetsLoaderService _assetsLoaderService;
        [Inject] private NetworkEventBus _networkEventBus;

        private Dictionary<string, BaseNetworkBehaviour> _behavioursById;

        public void Initialize()
        {
            _behavioursById = new();
        
            foreach (var handler in behavioursConfig.Behaviours)
            {
                if (handler == null) continue;

                var behaviour = _assetsLoaderService.LoadAssetSync<BaseNetworkBehaviour>(handler.Asset); //TODO: сделать прелоадом
                
                if (!string.IsNullOrEmpty(handler.Id))
                    _behavioursById[handler.Id] = behaviour;
            }
            
            _networkEventBus.SubscribeOnClients<BehaviourSpawnClientRequestBroadcast>(OnBehaviourSpawned);
        }
        
        private void OnBehaviourSpawned(BehaviourSpawnClientRequestBroadcast broadcast, Channel channel)
        {
            var behaviour = Create(broadcast.Id, broadcast.NetworkConnection,
                broadcast.Position, broadcast.Rotation, broadcast.Parent);
            
            var spawnedObjectArgs = new BehaviourSpawnServerResponseBroadcast
            {
                SpawnedObject = behaviour.gameObject
            };
            
            _networkEventBus.PublishTargetRpc(broadcast.NetworkConnection, spawnedObjectArgs);
        }
        
        public BaseNetworkBehaviour Create(string id, NetworkConnection conn, Vector3 position = default,
            Quaternion rotation = default, Transform parent = null)
        {
            var prefab = GetCreatureById(id);
            if (prefab == null)
                throw new InvalidOperationException($"Creature with ID '{id}' not found in config");

            var creature = Object.Instantiate(prefab, position, rotation, parent);
            InstanceFinder.ServerManager.Spawn(creature.gameObject, conn);
            InitializeCreature(creature);
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
        
        public void InitializeCreature<TCreature>(TCreature creature)
            where TCreature : MonoBehaviour, ICreature
        {
            _objectResolver.Inject(creature);
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