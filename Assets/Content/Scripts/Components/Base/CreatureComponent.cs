using System;
using Game.Creatures;
using Mirror;
using R3;

namespace Game.Components
{
    public abstract class CreatureComponent : NetworkBehaviour, IDisposable
    {
        protected NetworkCreature Creature;
        protected CompositeDisposable Disposable = new();
        
        public void Init(NetworkCreature creature)
        {
            Creature = creature;
        }

        public void Dispose()
        {
            Disposable.Dispose();
        }
    }
    
    public abstract class ClientCreatureComponent : CreatureComponent
    {
    }
    
    public abstract class ServerCreatureComponent : CreatureComponent
    {
    }
}