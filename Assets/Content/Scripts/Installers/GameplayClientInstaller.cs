using Game.Stages;

namespace Game.Installers
{
    public class GameplayClientInstaller : ClientInstaller
    {
        private PlayerSpawnRequestClientStage playerSpawnRequestClientStage;

        protected override void ConfigureStages()
        {
            playerSpawnRequestClientStage = new PlayerSpawnRequestClientStage(LocalConnection);
            
            AddStages(playerSpawnRequestClientStage);
        }
    }
}