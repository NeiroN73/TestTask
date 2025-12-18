using System.Collections.Generic;
using FishNet.Object;
using Game.Components;
using GameCore.Creatures;
using R3;
using UnityEngine;

namespace Game.Creatures
{
    public abstract class BaseNetworkBehaviour : NetworkBehaviour, ICreature
    {
        [field: SerializeField] public string Id { get; private set; }
        
        protected List<NetworkElement> Elements = new();
        protected CompositeDisposable Disposable = new();

        protected void AddElements(params NetworkElement[] elements)
        {
            Elements.AddRange(elements);
        }

        public abstract void Initialize();
    }
}