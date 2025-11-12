using System;
using Game.Creatures;
using Mirror;
using R3;

namespace Game.Components
{
    public abstract class CreatureComponent : NetworkBehaviour
    {
        protected NetworkCreature Creature;
        protected CompositeDisposable Disposable = new();
        
        public void TryInitialize(NetworkCreature creature)
        {
            Creature = creature;

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

    public abstract class SubCreatureComponent : CreatureComponent
    {
        
    }
}