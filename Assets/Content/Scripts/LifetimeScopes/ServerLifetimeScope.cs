using Content.Scripts.EventBus;
using Content.Scripts.Factories;
using GameCore.LifetimeScopes;

namespace Game.LifetimeScopes
{
    public class ServerLifetimeScope : BaseLifetimeScope
    {
        protected override void RegisterSystems()
        {
            Register<ServerEventBus>();
            
            Register<NetworkBehavioursServerFactory>();
        }
    }
}