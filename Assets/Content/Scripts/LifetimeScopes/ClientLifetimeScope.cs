using Content.Scripts.EventBus;
using GameCore.LifetimeScopes;

namespace Game.LifetimeScopes
{
    public class ClientLifetimeScope : BaseLifetimeScope
    {
        protected override void RegisterSystems()
        {
            Register<ClientEventBus>();
        }
    }
}