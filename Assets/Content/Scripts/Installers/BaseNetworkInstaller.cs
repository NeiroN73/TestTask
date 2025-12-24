using System;
using System.Collections.Generic;
using System.Linq;
using Content.Scripts.Factories;
using FishNet.Connection;
using FishNet.Object;
using Game.Creatures;
using Game.LifetimeScopes;
using Game.Services;
using Game.Stages;
using TriInspector;
using UnityEngine;
using VContainer;

namespace Game.Installers
{
    public class BaseNetworkInstaller : NetworkBehaviour
    {
        [SerializeField] private GameplayLifetimeScope _gameplayLifetimeScope;
        [SerializeField] private List<NetworkInstallerStage> _stages = new();
        [SerializeField] private List<NetworkService> _services = new();
        [SerializeField] private List<BaseNetworkBehaviour> _behaviours = new();
        
        private Dictionary<Type, NetworkInstallerStage> _stagesByType = new();
        private NetworkTickService _networkTickService;

        private T TryGetStage<T>() where T : NetworkInstallerStage
        {
            if (_stagesByType.TryGetValue(typeof(T), out var stage))
            {
                return (T)stage;
            }
            return null;
        }

        [Button]
        private void GatherNetworkSystems()
        {
            _gameplayLifetimeScope = FindAnyObjectByType<GameplayLifetimeScope>(FindObjectsInactive.Include);
            _stages = FindObjectsByType<NetworkInstallerStage>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
            _services = FindObjectsByType<NetworkService>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
            _behaviours = FindObjectsByType<BaseNetworkBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
        }

        public override void OnStartNetwork()
        {
            base.OnStartNetwork();
            
            _stagesByType = _stages.ToDictionary(s => s.GetType());
        }

        public override void OnSpawnServer(NetworkConnection connection)
        {
            base.OnSpawnServer(connection);

            _gameplayLifetimeScope = FindAnyObjectByType<GameplayLifetimeScope>(FindObjectsInactive.Include);
            _stages = FindObjectsByType<NetworkInstallerStage>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
            _services = FindObjectsByType<NetworkService>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
            _behaviours = FindObjectsByType<BaseNetworkBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
            
            _gameplayLifetimeScope.AddServices(_services);
            _gameplayLifetimeScope.Build();
            
            _stages.ForEach(s => _gameplayLifetimeScope.Container.Inject(s));
            _services.ForEach(s => _gameplayLifetimeScope.Container.Inject(s));
            _behaviours.ForEach(s => _gameplayLifetimeScope.Container.Inject(s));
            
            _networkTickService = _gameplayLifetimeScope.Container.Resolve<NetworkTickService>();
            
            InitializeClientTarget(connection);
        }

        [TargetRpc]
        private void InitializeClientTarget(NetworkConnection conn)
        {
            _gameplayLifetimeScope = FindAnyObjectByType<GameplayLifetimeScope>(FindObjectsInactive.Include);
            _stages = FindObjectsByType<NetworkInstallerStage>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
            _services = FindObjectsByType<NetworkService>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
            _behaviours = FindObjectsByType<BaseNetworkBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
            
            _gameplayLifetimeScope.AddServices(_services);
            _gameplayLifetimeScope.Build();
            
            NetworkObjectInitializeUtils.InitializeNetworkObjects(_services, _gameplayLifetimeScope.Container);
            NetworkObjectInitializeUtils.InitializeNetworkObjects(_stages, _gameplayLifetimeScope.Container);
            NetworkObjectInitializeUtils.InitializeNetworkObjects(_behaviours, _gameplayLifetimeScope.Container);
            
            _networkTickService = _gameplayLifetimeScope.Container.Resolve<NetworkTickService>();
            
            var playerSpawnStage = TryGetStage<PlayerSpawnRequestClientStage>();
            playerSpawnStage.Configure(conn);
            RunStages();
        }

        private void RunStages()
        {
            foreach (var stage in _stages)
            {
                stage.Run();
            }
        }

        private void Update()
        {
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
            _networkTickService?.ServerTick(Time.deltaTime); //todo: сделать через prediction
        }

        private void ClientTick()
        {
            _networkTickService?.ClientTick(Time.deltaTime);
            _networkTickService?.LocalClientTick(Time.deltaTime);
            _networkTickService?.OtherClientsTick(Time.deltaTime);
        }

        public override void OnStopNetwork()
        {
            base.OnStopNetwork();
            
            _networkTickService = null;
            _stagesByType.Clear();
        }
    }
}