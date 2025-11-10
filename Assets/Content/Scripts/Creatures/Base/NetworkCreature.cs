using System;
using System.Collections.Generic;
using System.Linq;
using Game.Components;
using GameCore.Creatures;
using Mirror;
using R3;
using UnityEngine;

namespace Game.Creatures
{
    public abstract class NetworkCreature : NetworkBehaviour, ICreature
    {
        [field: SerializeField] public string Id { get; private set; }
        
        protected Dictionary<Type, CreatureComponent> ComponentsByType = new();
        protected CompositeDisposable Disposable = new();
        
        [ReadOnly] private List<CreatureComponent> _components = new();

        public void TryInitialize()
        {
            _components = GetComponentsInChildren<CreatureComponent>().ToList();
            ComponentsByType = _components.ToDictionary(c => c.GetType());
            
            foreach (var (type, component) in ComponentsByType)
            {
                component.TryInitialize(this);
            }
            Initialize();
        }

        public void TryDispose()
        {
            foreach (var (type, component) in ComponentsByType)
            {
                component.Dispose();
            }
            Disposable.Dispose();
            Dispose();
        }

        public T GetCreatureComponentByType<T>() where T : CreatureComponent
        {
            if (ComponentsByType.TryGetValue(typeof(T), out var component))
            {
                return (T)component;
            }
            return null;
        }
        
        protected virtual void Initialize() {}
        protected virtual void Dispose() {}
    }
}