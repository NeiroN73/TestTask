using Content.Scripts.Factories;
using GameCore.LifetimeScopes;

namespace Game.LifetimeScopes
{
    public class ClientLifetimeScope : BaseLifetimeScope
    {
        protected override void RegisterFactories()
        {
            Register<NetworkBehavioursClientFactory>();
        }
    }
}