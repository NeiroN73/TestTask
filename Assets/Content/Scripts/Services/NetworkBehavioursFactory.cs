using System;
using System.Collections.Generic;
using FishNet;
using FishNet.Connection;
using Game.Behaviours;
using Game.NetworkInterfaces;
using GameCore.Configs;
using GameCore.Services;
using UnityEngine;
using VContainer;

namespace Game.Services
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
            
            var prefab = GetBehaviourById(id);
            if (prefab == null)
                throw new InvalidOperationException($"Behaviour with ID '{id}' not found in config");

            var behaviour = Instantiate(prefab, position, rotation, parent);
            InstanceFinder.ServerManager.Spawn(behaviour.gameObject, networkConnection);
            InitializeBehaviour(behaviour);
        }
        
        private BaseNetworkBehaviour GetBehaviourById(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
        
            if (_behavioursById.TryGetValue(id, out var behaviour))
            {
                return behaviour;
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