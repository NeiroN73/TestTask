using System;
using System.Collections.Generic;
using System.Linq;
using FishNet.Object;
using Game.Components;
using GameCore.Creatures;

using R3;
using TriInspector;
using UnityEngine;

namespace Game.Creatures
{
    public abstract class NetworkEntity : NetworkBehaviour, ICreature
    {
        [field: SerializeField] public string Id { get; private set; }
        
        protected Dictionary<Type, EntityComponent> EntityComponentsByType = new();
        protected CompositeDisposable Disposable = new();
        
        [ReadOnly] private List<EntityComponent> _entityComponents = new();

        public void TryInitialize()
        {
            _entityComponents = GetComponentsInChildren<EntityComponent>().ToList();
            EntityComponentsByType = _entityComponents.ToDictionary(c => c.GetType());
            
            foreach (var (type, component) in EntityComponentsByType)
            {
                component.TryInitialize(this);
            }
            Initialize();
        }

        public void TryDispose()
        {
            foreach (var (type, component) in EntityComponentsByType)
            {
                component.TryDispose();
            }
            Disposable.Dispose();
            Dispose();
        }

        public T GetEntityComponentByType<T>() where T : EntityComponent
        {
            if (EntityComponentsByType.TryGetValue(typeof(T), out var component))
            {
                return (T)component;
            }
            return null;
        }
        
        protected virtual void Initialize() {}
        protected virtual void Dispose() {}
    }
}