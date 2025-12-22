using System;
using System.Collections.Generic;
using Content.Scripts.EventBus;
using FishNet;
using FishNet.Connection;
using FishNet.Object;
using Game.Creatures;
using Game.LifetimeScopes;
using GameCore.Configs;
using GameCore.Services;
using UnityEngine;
using VContainer;
using Object = UnityEngine.Object;

namespace Content.Scripts.Factories
{
    public class NetworkBehavioursServerFactory : NetworkFactory, IServerInjectable, IServerInitializable
    {
        [Inject] private BehavioursConfig _behavioursConfig;
        [Inject] private AssetsLoaderService _assetsLoaderService;

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
        }
        
        [ServerRpc(RequireOwnership = false)]
        public void Create(string id, NetworkConnection networkConnection, Vector3 position = default,
            Quaternion rotation = default, Transform parent = null)
        {
            var prefab = GetCreatureById(id);
            if (prefab == null)
                throw new InvalidOperationException($"Creature with ID '{id}' not found in config");

            var creature = Instantiate(prefab, position, rotation, parent);
            InstanceFinder.ServerManager.Spawn(creature.gameObject, networkConnection);
            InitializeBehaviour(creature);
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
            behaviour.TryInitialize();
        }
    }
}