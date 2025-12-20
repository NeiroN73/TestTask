using System.Collections.Generic;
using FishNet.Object;
using Game.Creatures;
using Game.Stages;
using GameCore.LifetimeScopes;
using GameCore.Services;
using VContainer;

namespace Game.Installers
{
    public abstract class BaseNetworkInstaller : NetworkBehaviour
    {
        protected readonly List<NetworkInstallerStage> Stages = new();
        protected readonly List<BaseNetworkBehaviour> Behaviours = new();
        
        private TickService _tickService;
        private float _deltaTime;
        private bool _isTicked;
        
        protected void AddStages(params NetworkInstallerStage[] stages)
        {
            Stages.AddRange(stages);
        }
        
        protected virtual void ConfigureStages() {}

        protected void RunStages()
        {
            foreach (var stage in Stages)
            {
                stage.Run();
            }
        }

        protected void RunTick(BaseLifetimeScope scope, float deltaTime)
        {
            _tickService = scope.Container.Resolve<TickService>();
            _deltaTime = deltaTime;
            _isTicked = true;
        }

        private void Update()
        {
            if (_isTicked)
            {
                _tickService.Tick(_deltaTime);
            }
        }
    }
}