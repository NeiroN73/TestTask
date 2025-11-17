using System;
using Game.Creatures;
using Mirror;
using R3;

namespace Game.Components
{
    public abstract class EntityComponent : NetworkBehaviour
    {
        protected NetworkEntity Entity;
        protected CompositeDisposable Disposable = new();
        
        public void TryInitialize(NetworkEntity entity)
        {
            Entity = entity;

            Initialize();
        }
        
        public void TryDispose()
        {
            Disposable.Dispose();

            Dispose();
        }
        
        protected virtual void Initialize() {}
        protected virtual void Dispose() {}
    }
}