using Game.LifetimeScopes;
using UnityEngine;

namespace Game.Installers
{
    public class ServerInstaller : BaseNetworkInstaller
    {
        [SerializeField] private ServerLifetimeScope _serverLifetimeScope;
        
        public override void OnStartServer()
        {
            _serverLifetimeScope.Build();
            this.ProcessInterfacesFromContainer(_serverLifetimeScope);
            this.ProcessInterfaces(Behaviours, _serverLifetimeScope);
            ConfigureStages();
            this.ProcessInterfaces(Stages, _serverLifetimeScope);
            RunStages();
        }
    }
}