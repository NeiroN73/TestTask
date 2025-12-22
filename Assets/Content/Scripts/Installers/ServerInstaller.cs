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
            NetworkObjectInitializeUtils.InitializeObjectsFromContainer(_serverLifetimeScope);
            NetworkObjectInitializeUtils.InitializeServerObjects(Behaviours, _serverLifetimeScope);
            //ConfigureStages();
            NetworkObjectInitializeUtils.InitializeServerObjects(Stages, _serverLifetimeScope);
            RunTick(_serverLifetimeScope, Time.deltaTime);
            RunStages();
        }
    }
}