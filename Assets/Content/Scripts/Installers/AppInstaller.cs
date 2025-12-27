namespace Game.Installers
{
    public class AppInstaller : Installer
    {
        protected override void Initialize()
        {
            DontDestroyOnLoad(this);

            var entryPointInstaller = FindAnyObjectByType<EntryPointInstaller>();
            entryPointInstaller.Run();
        }
    }
}