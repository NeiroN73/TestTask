using Content.Scripts.EventBus;
using Content.Scripts.Factories;
using GameCore.LifetimeScopes;

namespace Game.LifetimeScopes
{
    public class ServerLifetimeScope : BaseLifetimeScope
    {
        public NetworkBehavioursFactory networkBehavioursFactory;
        
        protected override void RegisterSystems()
        {
            Register(networkBehavioursFactory);
        }
    }
}