using System.Collections.Generic;
using Content.Scripts.Factories;
using GameCore.LifetimeScopes;

namespace Game.LifetimeScopes
{
    public class GameplayLifetimeScope : BaseLifetimeScope
    {
        private List<NetworkService> _networkServices = new();
        
        protected override void RegisterSystems()
        {
            foreach (var networkService in _networkServices)
            {
                Register(networkService);
            }
        }

        public void AddServices(List<NetworkService> networkServices)
        {
            _networkServices = networkServices;
        }
    }
}