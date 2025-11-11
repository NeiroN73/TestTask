using GameCore.Services;
using VContainer;

namespace Game.Installers
{
    public class EntryPointInstaller : Installer
    {
        [Inject] private ScenesService _scenesService;
        
        public async void Run()
        {
            LifetimeScope.Build();
            
            await _scenesService.LoadSceneAsync(SceneConsts.MainMenu);
        }
    }
}