using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Game.Creatures;
using Game.LifetimeScopes;
using Game.Stages;
using Mirror;
using R3;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using IInitializable = GameCore.Services.IInitializable;
using ITickable = GameCore.Services.ITickable;

public class RunNetworkStage : NetworkInstallerStage
{
    [SerializeField] private ServerLifetimeScope _serverLifetimeScope;
    [SerializeField] private ClientLifetimeScope _clientLifetimeScope;
    
    [Inject] private SpawnPlayerStage _spawnPlayerStage;
    
    private readonly List<NetworkBehaviour> _networkBehaviours = new();
    
    private readonly List<IClientsTickable> _clientsTickables = new();
    private readonly List<IClientsDisposable> _clientsDisposables = new();
    
    private readonly List<IServerTickable> _serverTickables = new();
    private readonly List<IServerDisposable> _serverDisposables = new();
    
    private readonly List<ILocalClientTickable> _localClientTickables = new();
    private readonly List<ILocalClientDisposable> _localClientDisposables = new();
    
    private readonly List<IOtherClientsTickable> _otherClientsTickables = new();
    private readonly List<IOtherClientsDisposable> _otherClientsDisposables = new();

    public override void Initialize()
    {
        _spawnPlayerStage.PlayerSpawned.Subscribe(PlayerSpawned).AddTo(Disposables);
    }

    public async override UniTask Run()
    {
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
            if (networkBehaviour is IServerDisposable serverDisposable)
            {
                _serverDisposables.Add(serverDisposable);
            }
        }
        
        await UniTask.CompletedTask;
    }

    private void PlayerSpawned(NetworkIdentity networkIdentity)
    {
        InitializeClientTarget(networkIdentity.connectionToClient);
    }
    
    [TargetRpc]
    private void InitializeClientTarget(NetworkConnectionToClient conn)
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
            if (networkBehaviour is IClientsTickable clientsTickable)
            {
                _clientsTickables.Add(clientsTickable);
            }
            if (networkBehaviour is IClientsDisposable clientsDisposable)
            {
                _clientsDisposables.Add(clientsDisposable);
            }

            if (networkBehaviour.authority)
            {
                if (networkBehaviour is ILocalClientInitializable localClientInitializable)
                {
                    localClientInitializable.LocalClientInitialize();
                }
                if (networkBehaviour is ILocalClientTickable localClientTickable)
                {
                    _localClientTickables.Add(localClientTickable);
                }
                if (networkBehaviour is ILocalClientDisposable localClientDisposable)
                {
                    _localClientDisposables.Add(localClientDisposable);
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
                if (networkBehaviour is IOtherClientsDisposable otherClientsDisposable)
                {
                    _otherClientsDisposables.Add(otherClientsDisposable);
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
        for (int i = 0; i < _clientsTickables.Count; i++)
        {
            _clientsTickables[i].ClientsTick(Time.deltaTime);
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

    public override void OnStopServer()
    {
        foreach (var serverDisposable in _serverDisposables)
        {
            serverDisposable.ServerDispose();
        }
        foreach (var otherClientsDisposable in _otherClientsDisposables)
        {
            otherClientsDisposable.OtherClientsDispose();
        }
    }

    public override void OnStopClient()
    {
        foreach (var clientsDisposable in _clientsDisposables)
        {
            clientsDisposable.ClientsDispose();
        }
    }

    public override void OnStopLocalPlayer()
    {
        foreach (var localClientDisposable in _localClientDisposables)
        {
            localClientDisposable.LocalClientDispose();
        }
    }

    public override void Dispose()
    {
        foreach (var serverDisposable in _serverDisposables)
        {
            serverDisposable.ServerDispose();
        }
    }
}