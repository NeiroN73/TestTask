using Content.Scripts.EventBus;
using Content.Scripts.Factories;
using GameCore.LifetimeScopes;

namespace Game.LifetimeScopes
{
    public class ClientLifetimeScope : BaseLifetimeScope
    {
        protected override void RegisterSystems()
        {
            Register<ClientEventBus>();
            
            Register<NetworkBehavioursClientFactory>();
        }
    }
}