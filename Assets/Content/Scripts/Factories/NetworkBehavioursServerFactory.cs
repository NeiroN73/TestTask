using System;
using System.Collections.Generic;
using FishNet;
using FishNet.Broadcast;
using FishNet.Connection;
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
        [Inject] private CreaturesConfig _creaturesConfig;
        [Inject] private AssetsLoaderService _assetsLoaderService;

        private Dictionary<string, ICreature> _creaturesById;
        private Dictionary<Type, ICreature> _creaturesByType;

        public void ServerInitialize()
        {
            _creaturesById = new();
            _creaturesByType = new();
        
            foreach (var handler in _creaturesConfig.Creatures)
            {
                if (handler == null) continue;

                var asset = _assetsLoaderService.LoadAssetSync<ICreature>(handler.Asset); //TODO: сделать прелоадом
                
                if (!string.IsNullOrEmpty(asset.Id))
                    _creaturesById[asset.Id] = asset;
                
                _creaturesByType[asset.GetType()] = asset;
            }
        }

        public TCreature Create<TCreature>(string id, NetworkConnection conn, Vector3 position = default, Quaternion rotation = default, Transform parent = null) 
            where TCreature : MonoBehaviour, ICreature
        {
            var prefab = GetCreatureById<TCreature>(id);
            if (prefab == null)
                throw new InvalidOperationException($"Creature with ID '{id}' and type {typeof(TCreature)} not found in config");

            return Create(prefab, conn, position, rotation, parent);
        }

        public TCreature Create<TCreature>(TCreature prefab, NetworkConnection conn, Vector3 position = default, Quaternion rotation = default, Transform parent = null) 
            where TCreature : MonoBehaviour, ICreature
        {
            if (prefab == null)
                throw new ArgumentNullException(nameof(prefab));

            var creature = Object.Instantiate(prefab, position, rotation, parent);
            InstanceFinder.ServerManager.Spawn(creature.gameObject, conn);
            InitializeCreature(creature);
            return creature;
        }
        
        private TCreature GetCreatureById<TCreature>(string id) where TCreature : MonoBehaviour
        {
            if (string.IsNullOrEmpty(id)) return null;
        
            if (_creaturesById.TryGetValue(id, out var creature) && 
                creature is TCreature typedCreature)
            {
                return typedCreature;
            }
            return null;
        }
        
        public void InitializeCreature<TCreature>(TCreature creature)
            where TCreature : MonoBehaviour, ICreature
        {
            _objectResolver.Inject(creature);
        }
    }

    public struct BehaviourSpawnArgs : IBroadcast
    {
        public string Id;
        public NetworkConnection NetworkConnection;
        public Vector3 Position;
        public Quaternion Rotation;
        public Transform Parent;
    }
}