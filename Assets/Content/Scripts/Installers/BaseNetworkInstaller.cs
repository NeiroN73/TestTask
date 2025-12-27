using System;
using System.Collections.Generic;
using System.Linq;
using FishNet.Connection;
using FishNet.Object;
using Game.Behaviours;
using Game.LifetimeScopes;
using Game.Services;
using Game.Stages;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Installers
{
    public class BaseNetworkInstaller : NetworkBehaviour
    {
        private GameplayLifetimeScope _gameplayLifetimeScope;
        private List<NetworkInstallerStage> _stages = new();
        private List<NetworkService> _services = new();
        private List<BaseNetworkBehaviour> _behaviours = new();
        
        private Dictionary<Type, NetworkInstallerStage> _stagesByType = new();
        private NetworkTickService _networkTickService;
        
        private bool _isInitialized;
        
        public override void OnSpawnServer(NetworkConnection connection)
        {
            base.OnSpawnServer(connection);
            
            InitializeClientTarget(connection);
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            
            _gameplayLifetimeScope = LifetimeScope.Find<GameplayLifetimeScope>() as GameplayLifetimeScope;
            _stages = FindObjectsByType<NetworkInstallerStage>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
            _services = FindObjectsByType<NetworkService>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
            _behaviours = FindObjectsByType<BaseNetworkBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
            
            _stagesByType = _stages.ToDictionary(s => s.GetType());
            
            _gameplayLifetimeScope.AddServices(_services);
            _gameplayLifetimeScope.Build();
            
            NetworkObjectInitializeUtils.InitializeNetworkObjects(_services, _gameplayLifetimeScope.Container);
            NetworkObjectInitializeUtils.InitializeNetworkObjects(_stages, _gameplayLifetimeScope.Container);
            NetworkObjectInitializeUtils.InitializeNetworkObjects(_behaviours, _gameplayLifetimeScope.Container);
            
            _networkTickService = _gameplayLifetimeScope.Container.Resolve<NetworkTickService>();
        }

        protected T TryGetStage<T>() where T : NetworkInstallerStage
        {
            if (_stagesByType.TryGetValue(typeof(T), out var stage))
            {
                return (T)stage;
            }
            return null;
        }
        
        protected virtual void ConfigureStages(NetworkConnection conn) {}
        
        [TargetRpc]
        private void InitializeClientTarget(NetworkConnection conn)
        {
            ConfigureStages(conn);
            RunStages();
        }
        
        private async void RunStages()
        {
            foreach (var stage in _stages)
            {
                await stage.Run();
            }

            _isInitialized = true;
        }

        private void Update()
        {
            if (!_isInitialized)
                return;
            
            if (IsServerInitialized)
            {
                ServerTick();
            }
            if (IsClientInitialized)
            {
                ClientTick();
            }
        }

        private void ServerTick()
        {
            _networkTickService.ServerTick(Time.deltaTime);
        }

        private void ClientTick()
        {
            _networkTickService.ClientTick(Time.deltaTime);
            _networkTickService.LocalClientTick(Time.deltaTime);
            _networkTickService.OtherClientsTick(Time.deltaTime);
        }

        public override void OnStopNetwork()
        {
            base.OnStopNetwork();
            
            NetworkObjectInitializeUtils.DisposeNetworkObjects(_services, _gameplayLifetimeScope.Container);
            NetworkObjectInitializeUtils.DisposeNetworkObjects(_stages, _gameplayLifetimeScope.Container);
            NetworkObjectInitializeUtils.DisposeNetworkObjects(_behaviours, _gameplayLifetimeScope.Container);
            
            _networkTickService = null;
            _stagesByType.Clear();
        }
    }
}