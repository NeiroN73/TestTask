using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Game.Creatures;
using Game.LifetimeScopes;
using Game.Stages;
using Mirror;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using IInitializable = GameCore.Services.IInitializable;
using ITickable = GameCore.Services.ITickable;

public class RunNetworkStage : NetworkInstallerStage
{
    [SerializeField] private ServerLifetimeScope _serverLifetimeScope;
    [SerializeField] private ClientLifetimeScope _clientLifetimeScope;
    
    private NetworkCreature[] _serverNetworkCreature;
    private NetworkCreature[] _clientNetworkCreature;
    
    private ITickable[] _serverTickables;
    private ITickable[] _clientTickables;
    
    public async override UniTask Run()
    {
        if (NetworkServer.active)
        {
            _serverNetworkCreature =
                FindObjectsByType<NetworkCreature>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            InitializeSide(_serverLifetimeScope, _serverNetworkCreature, out _serverTickables);
        }
        
        if (NetworkClient.active)
        {
            if (NetworkServer.active)
            {
                _clientNetworkCreature =
                    FindObjectsByType<NetworkCreature>(FindObjectsInactive.Include, FindObjectsSortMode.None);
                InitializeSide(_clientLifetimeScope, _clientNetworkCreature, out _clientTickables);
            }
            else
            {
                InitializeClientTarget(NetworkClient.localPlayer.connectionToClient);
            }
        }
        
        await UniTask.CompletedTask;
    }
    
    [TargetRpc]
    private void InitializeClientTarget(NetworkConnectionToClient conn)
    {
        InitializeSide(_clientLifetimeScope,
            FindObjectsByType<NetworkCreature>(FindObjectsInactive.Include, FindObjectsSortMode.None), out _clientTickables);
    }
    
    private void InitializeSide<T>(LifetimeScope scope, T[] creatures, out ITickable[] tickables) where T : NetworkCreature
    {
        scope.Build();
        
        var initializables = scope.Container.Resolve<IEnumerable<IInitializable>>();
        
        foreach (var initializable in initializables)
        {
            initializable.Initialize();
        }
        
        tickables = scope.Container.Resolve<IEnumerable<ITickable>>().ToArray();
        
        foreach (var creature in creatures)
        {
            scope.Container.Inject(creature);
            creature.TryInitialize();
        }
    }
    
    private void Update()
    {
        if (NetworkServer.active)
        {
            for (int i = 0; i < _serverTickables?.Length; i++)
            {
                _serverTickables[i].Tick(Time.deltaTime);
            }
        }
        
        if (NetworkClient.active)
        {
            for (int i = 0; i < _clientTickables?.Length; i++)
            {
                _clientTickables[i].Tick(Time.deltaTime);
            }
        }
    }

    public override void Dispose()
    {
        if (NetworkServer.active)
        {
            foreach (var creature in _serverNetworkCreature)
            {
                creature.TryDispose();
            }
        }
        
        if (NetworkClient.active)
        {
            foreach (var creature in _clientNetworkCreature)
            {
                creature.TryDispose();
            }
        }
    }
}