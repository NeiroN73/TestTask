using FishNet.Connection;
using FishNet.Object;
using Game.LifetimeScopes;
using UnityEngine;

namespace Game.Installers
{
    public class ClientInstaller : BaseNetworkInstaller
    {
        [SerializeField] private ClientLifetimeScope _clientLifetimeScope;

        public override void OnSpawnServer(NetworkConnection connection)
        {
            InitializeClientTarget(connection);
        }
        
        [TargetRpc]
        private void InitializeClientTarget(NetworkConnection conn)
        {
            _clientLifetimeScope.Build();
            this.ProcessInterfacesFromContainer(_clientLifetimeScope);
            this.ProcessInterfaces(Behaviours, _clientLifetimeScope);
            ConfigureStages();
            this.ProcessInterfaces(Stages, _clientLifetimeScope);
            RunStages();
        }
    }
}