using Content.Scripts.EventBus;
using Content.Scripts.Factories;
using GameCore.LifetimeScopes;

namespace Game.LifetimeScopes
{
    public class ClientLifetimeScope : BaseLifetimeScope
    {
        public NetworkBehavioursClientFactory  NetworkBehavioursClientFactory;
        
        protected override void RegisterSystems()
        {
            Register<ClientEventBus>();
            
            Register(NetworkBehavioursClientFactory);
        }
    }
}