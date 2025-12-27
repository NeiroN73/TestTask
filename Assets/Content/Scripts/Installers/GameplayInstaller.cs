using FishNet.Connection;
using Game.Stages;

namespace Game.Installers
{
    public class GameplayInstaller : BaseNetworkInstaller
    {
        protected override void ConfigureStages(NetworkConnection conn)
        {
            base.ConfigureStages(conn);
            
            TryGetStage<PlayerSpawnRequestClientStage>().Configure(conn);
        }
    }
}