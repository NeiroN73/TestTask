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
            NetworkObjectInitializeUtils.InitializeObjectsFromContainer(_clientLifetimeScope, IsController);
            NetworkObjectInitializeUtils.InitializeClientObjects(Behaviours, _clientLifetimeScope, IsController);
            ConfigureStages();
            NetworkObjectInitializeUtils.InitializeClientObjects(Stages, _clientLifetimeScope, IsController);
            RunTick(_clientLifetimeScope, Time.deltaTime);
            RunStages();
        }
    }
}