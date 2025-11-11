using Game.UI.MainMenu;
using GameCore.Services;
using VContainer;

namespace Game.Installers
{
    public class MainMenuInstaller : Installer
    {
        [Inject] private ScreensService _screensService;

        protected async override void Initialize()
        {
            LifetimeScope.Build();
            
            await _screensService.OpenAsync<MainMenuScreen>();
        }
    }
}