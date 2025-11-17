using Game.Creatures;
using GameCore.Factories;
using Mirror;
using R3;
using VContainer;

namespace Game.Components
{
    public class SpawnComponent : EntityComponent
    {
        [Inject] private CreaturesFactory creaturesFactory;

        public void Init(ControllerComponent controllerComponent)
        {
            controllerComponent.SpawnPerformed.Subscribe(CmdSpawnCube).AddTo(Disposable);
        }

        [Command]
        private void CmdSpawnCube()
        {
            var spawnPosition = transform.position + transform.forward * 2f;
            var cube = creaturesFactory.Create<CubeEntity>(spawnPosition);
            NetworkServer.Spawn(cube.gameObject);
        }
    }
}