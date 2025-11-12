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
        
        protected Dictionary<Type, CreatureComponent> CreatureComponentsByType = new();
        protected CompositeDisposable Disposable = new();
        
        [ReadOnly] private List<CreatureComponent> _creatureComponents = new();
        
        private List<SubNetworkCreature> _subNetworkCreatures = new();
        protected Dictionary<Type, SubNetworkCreature> SubNetworkCreaturesByType = new();

        public void TryInitialize()
        {
            _creatureComponents = GetComponentsInChildren<CreatureComponent>().ToList();
            _subNetworkCreatures = GetComponentsInChildren<SubNetworkCreature>().ToList();
            CreatureComponentsByType = _creatureComponents.ToDictionary(c => c.GetType());
            SubNetworkCreaturesByType = _subNetworkCreatures.ToDictionary(c => c.GetType());
            
            foreach (var (type, component) in CreatureComponentsByType)
            {
                component.TryInitialize(this);
            }
            Initialize();
        }

        public void TryDispose()
        {
            foreach (var (type, component) in CreatureComponentsByType)
            {
                component.TryDispose();
            }
            Disposable.Dispose();
            Dispose();
        }

        public T GetCreatureComponentByType<T>() where T : CreatureComponent
        {
            if (CreatureComponentsByType.TryGetValue(typeof(T), out var component))
            {
                return (T)component;
            }
            return null;
        }
        
        public T GetSubCreatureByType<T>() where T : SubNetworkCreature
        {
            if (SubNetworkCreaturesByType.TryGetValue(typeof(T), out var subCreature))
            {
                return (T)subCreature;
            }
            return null;
        }
        
        protected virtual void Initialize() {}
        protected virtual void Dispose() {}
    }

    public abstract class SubNetworkCreature : NetworkCreature
    {
        
    }
}