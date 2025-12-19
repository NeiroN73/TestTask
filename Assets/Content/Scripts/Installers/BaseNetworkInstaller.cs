using System.Collections.Generic;
using FishNet.Object;
using Game.Creatures;
using Game.Stages;

namespace Game.Installers
{
    public abstract class BaseNetworkInstaller : NetworkBehaviour
    {
        protected readonly List<NetworkInstallerStage> Stages = new();
        protected readonly List<BaseNetworkBehaviour> Behaviours = new();
        
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
    }
}