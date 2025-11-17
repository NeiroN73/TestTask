using System.Collections.Generic;
using System.Linq;
using Game.Stages;
using Mirror;

namespace Game.Installers
{
    public abstract class NetworkInstaller : NetworkBehaviour
    {
        private List<NetworkInstallerStage> _stages = new();

        public override void OnStartServer()
        {
            DontDestroyOnLoad(this);

            GatherStages();
            RunStages();
        }

        private void GatherStages()
        {
            _stages = GetComponentsInChildren<NetworkInstallerStage>().ToList();
            
            foreach (var stage in _stages)
            {
                stage.Initialize();
            }
        }

        private async void RunStages()
        {
            foreach (var stage in _stages)
            {
                await stage.Run();
            }
        }

        public override void OnStopServer()
        {
            foreach (var stage in _stages)
            {
                stage.Dispose();
            }
        }
    }
}