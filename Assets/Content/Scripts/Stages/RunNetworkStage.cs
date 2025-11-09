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

public class RunNetworkStage : InstallerStage
{
    [SerializeField] private ServerLifetimeScope _serverLifetimeScope;
    [SerializeField] private ClientLifetimeScope _clientLifetimeScope;
    
    private ITickable[] _serverTickables;
    private ITickable[] _clientTickables;
    
    public async override UniTask Initialize()
    {
        if (NetworkServer.active)
        {
            _serverTickables = InitializeSide(_serverLifetimeScope, 
                FindObjectsByType<ServerNetworkCreature>(FindObjectsInactive.Include, FindObjectsSortMode.None));
        }
        
        if (NetworkClient.active)
        {
            if (NetworkServer.active)
            {
                _clientTickables = InitializeSide(_clientLifetimeScope,
                    FindObjectsByType<ClientNetworkCreature>(FindObjectsInactive.Include, FindObjectsSortMode.None));
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
            creature.Initialize();
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
}