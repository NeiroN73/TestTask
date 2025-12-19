using Game.Stages;

namespace Game.Installers
{
    public class GameplayClientInstaller : ClientInstaller
    {
        private PlayerClientSpawnRequestStage _playerClientSpawnRequestStage;

        protected override void ConfigureStages()
        {
            _playerClientSpawnRequestStage = new PlayerClientSpawnRequestStage(Owner);
            
            AddStages(_playerClientSpawnRequestStage);
        }
    }
}