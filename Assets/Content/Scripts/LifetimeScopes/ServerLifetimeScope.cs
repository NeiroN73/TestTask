using Content.Scripts.Factories;
using GameCore.LifetimeScopes;

namespace Game.LifetimeScopes
{
    public class ServerLifetimeScope : BaseLifetimeScope
    {
        protected override void RegisterFactories()
        {
            Register<NetworkBehavioursServerFactory>();
        }
    }
}