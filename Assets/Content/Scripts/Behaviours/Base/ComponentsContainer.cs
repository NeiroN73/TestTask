using System;
using System.Collections.Generic;
using Game.Components;
using UnityEngine;

namespace Game.Behaviours
{
    public class ComponentsContainer
    {
        private readonly GameObject _gameObject;
        
        private Dictionary<Type, NetworkComponent> _componentsByType = new();
        public IReadOnlyCollection<NetworkComponent> Components => _componentsByType.Values;

        public ComponentsContainer(GameObject gameObject)
        {
            _gameObject = gameObject;
        }
        
        public bool TryGetNetworkComponent<T>(out T outComponent) where T : NetworkComponent
        {
            if (_componentsByType.TryGetValue(typeof(T), out var component))
            {
                outComponent = component as T;
                return true;
            }
            
            outComponent = null;
            return false;
        }

        public T TryAddNetworkComponent<T>() where T : NetworkComponent
        {
            var type = typeof(T);
            
            if (_gameObject.TryGetComponent<T>(out var outComponent))
            {
                if (_componentsByType.TryAdd(type, outComponent))
                {
                    return outComponent;
                }
                return null;
            }
#if UNITY_EDITOR
            else
            {
                return _gameObject.AddComponent<T>();
            }
#else
            return null;
#endif
        }
        
        public TComponent TryAddNetworkComponent<TComponent, TBaseComponent>()
            where TComponent : NetworkComponent
            where TBaseComponent : NetworkComponent
        {
            if (_gameObject.GetComponent<TComponent>())
            {
                return TryAddNetworkComponent<TBaseComponent>() as TComponent;
            }
#if UNITY_EDITOR
            else
            {
                return _gameObject.AddComponent<TComponent>();
            }
#else
            return null;
#endif
        }
    }
}