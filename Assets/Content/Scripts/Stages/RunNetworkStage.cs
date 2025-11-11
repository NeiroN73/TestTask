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
    
    private ITickable[] _serverTickables;
    private ITickable[] _clientTickables;
    
    private ServerNetworkCreature[] _serverNetworkCreature;
    private ClientNetworkCreature[] _clientNetworkCreature;
    
    public async override UniTask Run()
    {
        if (NetworkServer.active)
        {
            _serverNetworkCreature =
                FindObjectsByType<ServerNetworkCreature>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            _serverTickables = InitializeSide(_serverLifetimeScope, _serverNetworkCreature);
        }
        
        if (NetworkClient.active)
        {
            if (NetworkServer.active)
            {
                _clientNetworkCreature =
                    FindObjectsByType<ClientNetworkCreature>(FindObjectsInactive.Include, FindObjectsSortMode.None);
                _clientTickables = InitializeSide(_clientLifetimeScope, _clientNetworkCreature);
            }
            else
            {
                InitializeClientForRemoteClient(NetworkClient.localPlayer.connectionToClient);
            }
        }
        
        await UniTask.CompletedTask;
    }
    
    [TargetRpc]
    private void InitializeClientForRemoteClient(NetworkConnectionToClient conn)
    {
        _clientTickables = InitializeSide(_clientLifetimeScope,
            FindObjectsByType<ClientNetworkCreature>(FindObjectsInactive.Include, FindObjectsSortMode.None));
    }
    
    private ITickable[] InitializeSide<T>(LifetimeScope scope, T[] creatures) where T : NetworkCreature
    {
        scope.Build();
        
        var initializables = scope.Container.Resolve<IEnumerable<IInitializable>>();
        
        foreach (var initializable in initializables)
        {
            initializable.Initialize();
        }
        
        var tickables = scope.Container.Resolve<IEnumerable<ITickable>>().ToArray();
        
        foreach (var creature in creatures)
        {
            scope.Container.Inject(creature);
            creature.TryInitialize();
        }

        return tickables;
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
    }
}