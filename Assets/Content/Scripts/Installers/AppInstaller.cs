using UnityEngine;

namespace Game.Installers
{
    public class AppInstaller : Installer
    {
        [SerializeField] private EntryPointInstaller _entryPointInstaller;

        protected override void Initialize()
        {
            DontDestroyOnLoad(this);

            _entryPointInstaller.Run();
        }
    }
}