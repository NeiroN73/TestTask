using System.Collections.Generic;
using Game.Creatures;
using Game.Stages;
using GameCore.LifetimeScopes;
using GameCore.Services;
using Mirror;
using UnityEngine;
using VContainer;

namespace Game.Installers
{
    public class GameInstaller : NetworkBehaviour
    {
        [SerializeField] private BaseLifetimeScope _lifetimeScopeServer;
        [SerializeField] private BaseLifetimeScope _lifetimeScopeClient;
        
        [Inject] private ScenesService _scenesService;
        
        [Inject] private SpawnPlayerStage _spawnPlayerStage;
        [Inject] private RunNetworkStage _runNetworkStage;
        
        private List<InstallerStage> _stages = new();
        
        private Creature[] _creatures;
        private NetworkCreature[] _networkCreatures;

        public override void OnStartServer()
        {
            DontDestroyOnLoad(this);

            BuildStages();
        }

        private void BuildStages()
        {
            _stages.Add(_spawnPlayerStage);
            _stages.Add(_runNetworkStage);
        }
    }
}