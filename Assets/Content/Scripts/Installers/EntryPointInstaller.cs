using Game.Services;
using GameCore.Services;
using VContainer;

namespace Game.Installers
{
    public class EntryPointInstaller : Installer
    {
        [Inject] private ScenesService _scenesService;
        [Inject] private ConnectionService _connectionService;
        
        public async void Run()
        {
            LifetimeScope.Build();
            
            #if UNITY_SERVER
                await _connectionService.HostGameAsync(null);
            #else
                await _scenesService.LoadSceneAsync(SceneConsts.MainMenu);
            #endif
        }
    }
}