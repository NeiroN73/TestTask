using System.Collections.Generic;
using System.Linq;
using Content.Scripts.EventBus;
using Cysharp.Threading.Tasks;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Transporting;
using Game.Creatures;
using Game.LifetimeScopes;
using Game.NetworkManagers;
using Game.Stages;

using R3;
using UnityEngine;
using VContainer;

namespace Game.Installers
{
    public abstract class NetworkInstaller : NetworkBehaviour
    {
        [SerializeField] private ServerLifetimeScope _serverLifetimeScope;
        [SerializeField] private ClientLifetimeScope _clientLifetimeScope;
        
        private readonly List<NetworkInstallerStage> _stages = new();
        private readonly List<NetworkBehaviour> _networkBehaviours = new();
        
        private readonly List<IClientTickable> _clientTickables = new();
        private readonly List<IServerTickable> _serverTickables = new();
        private readonly List<ILocalClientTickable> _localClientTickables = new();
        private readonly List<IOtherClientsTickable> _otherClientsTickables = new();

        private readonly CompositeDisposable _disposable = new();
        
        public override void OnStartServer()
        {
            NetworkManager.ServerManager.OnRemoteConnectionState += OnRemoteConnectionStateChanged;
            
            _serverLifetimeScope.Build();
            foreach (var networkBehaviour in _networkBehaviours)
            {
                if (networkBehaviour is IServerInjectable serverInjectable)
                {
                    _serverLifetimeScope.Container.Inject(serverInjectable);
                }
                if (networkBehaviour is IServerInitializable serverInitializable)
                {
                    serverInitializable.ServerInitialize();
                }
                if (networkBehaviour is IServerTickable serverTickable)
                {
                    _serverTickables.Add(serverTickable);
                }
            }
            
            GatherStages();
            RunStages();
        }

        private void OnRemoteConnectionStateChanged(NetworkConnection networkConnection, RemoteConnectionStateArgs remoteConnectionStateArgs)
        {
            if (remoteConnectionStateArgs.ConnectionState == RemoteConnectionState.Started)
            {
                Debug.Log($"Клиент подключился: {networkConnection.ClientId}");
                OnClientConnected(networkConnection);
            }
            else if (remoteConnectionStateArgs.ConnectionState == RemoteConnectionState.Stopped)
            {
                Debug.Log($"Клиент отключился: {networkConnection.ClientId}");
                OnClientDisconnected(networkConnection);
            }
        }
        
        private void OnClientConnected(NetworkConnection networkConnection)
        {
            InitializeClientTarget(networkConnection);
        }
        
        private void GatherStages()
        {
            _stages.Clear();
            _stages.AddRange(GetComponentsInChildren<NetworkInstallerStage>());
            
            foreach (var stage in _stages)
            {
                stage.ServerInitialize();
            }
        }

        private async void RunStages()
        {
            foreach (var stage in _stages)
            {
                await stage.ServerRun();
            }
        }
        
        [TargetRpc]
        private void InitializeClientTarget(NetworkConnection conn)
        {
            _clientLifetimeScope.Build();
            foreach (var networkBehaviour in _networkBehaviours)
            {
                if (networkBehaviour is IClientsInjectable clientsInjectable)
                {
                    _serverLifetimeScope.Container.Inject(clientsInjectable);
                }
                if (networkBehaviour is IClientsInitializable clientsInitializable)
                {
                    clientsInitializable.ClientsInitialize();
                }
                if (networkBehaviour is IClientTickable clientsTickable)
                {
                    _clientTickables.Add(clientsTickable);
                }

                if (networkBehaviour.IsController)
                {
                    if (networkBehaviour is ILocalClientInitializable localClientInitializable)
                    {
                        localClientInitializable.LocalClientInitialize();
                    }
                    if (networkBehaviour is ILocalClientTickable localClientTickable)
                    {
                        _localClientTickables.Add(localClientTickable);
                    }
                }
                else
                {
                    if (networkBehaviour is IOtherClientsInitializable otherClientsInitializable)
                    {
                        otherClientsInitializable.OtherClientsInitialize();
                    }
                    if (networkBehaviour is IOtherClientsTickable otherClientsTickable)
                    {
                        _otherClientsTickables.Add(otherClientsTickable);
                    }
                }
            }
        }
        
        private void Update()
        {
            for (int i = 0; i < _serverTickables.Count; i++)
            {
                _serverTickables[i].ServerTick(Time.deltaTime);
            }
            for (int i = 0; i < _clientTickables.Count; i++)
            {
                _clientTickables[i].ClientTick(Time.deltaTime);
            }
            for (int i = 0; i < _localClientTickables.Count; i++)
            {
                _localClientTickables[i].LocalClientTick(Time.deltaTime);
            }
            for (int i = 0; i < _otherClientsTickables.Count; i++)
            {
                _otherClientsTickables[i].OtherClientsTick(Time.deltaTime);
            }
        }
        
        private void OnClientDisconnected(NetworkConnection networkConnection)
        {
        }
    }
}