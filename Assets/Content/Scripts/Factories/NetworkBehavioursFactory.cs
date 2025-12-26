using System;
using System.Collections.Generic;
using FishNet;
using FishNet.Connection;
using FishNet.Object;
using Game.Creatures;
using GameCore.Configs;
using GameCore.Services;
using UnityEngine;
using VContainer;

namespace Content.Scripts.Factories
{
    public class NetworkBehavioursFactory : NetworkService, IServerInitializable, IInjectable
    {
        [Inject] private BehavioursConfig _behavioursConfig;
        [Inject] private AssetsLoaderService _assetsLoaderService;
        [Inject] private IObjectResolver _objectResolver;

        private Dictionary<string, BaseNetworkBehaviour> _behavioursById;

        public void ServerInitialize()
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
        
        public void Create(string id, Vector3 position = default, Quaternion rotation = default,
            Transform parent = null, NetworkConnection networkConnection = null)
        {
            if(rotation == default)
                rotation = Quaternion.identity;
            
            var prefab = GetCreatureById(id);
            if (prefab == null)
                throw new InvalidOperationException($"Creature with ID '{id}' not found in config");

            var behaviour = Instantiate(prefab, position, rotation, parent);
            InstanceFinder.ServerManager.Spawn(behaviour.gameObject, networkConnection);
            InitializeBehaviour(behaviour);
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
        
        public void InitializeBehaviour(BaseNetworkBehaviour behaviour)
        {
            _objectResolver.Inject(behaviour);
            behaviour.Initialize(_objectResolver);
        }
    }
}