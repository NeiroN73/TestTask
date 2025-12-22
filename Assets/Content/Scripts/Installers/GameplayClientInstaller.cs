using FishNet.Connection;
using Game.Stages;

namespace Game.Installers
{
    public class GameplayClientInstaller : ClientInstaller
    {
        protected override void ConfigureStages(NetworkConnection networkConnection)
        {
            var stage = FindAnyObjectByType<PlayerSpawnRequestClientStage>();
            stage.Initialize(networkConnection);
            
            AddStages(stage);
        }
    }
}