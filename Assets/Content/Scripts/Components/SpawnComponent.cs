using FishNet.Object;
using Game.Creatures;
using GameCore.Factories;

using R3;
using VContainer;

namespace Game.Components
{
    public class SpawnComponent : EntityComponent
    {
        [Inject] private CreaturesFactory creaturesFactory;

        public void Init(ControllerElement controllerElement)
        {
            controllerElement.SpawnPerformed.Subscribe(CmdSpawnCube).AddTo(Disposable);
        }

        [ServerRpc]
        private void CmdSpawnCube()
        {
            var spawnPosition = transform.position + transform.forward * 2f;
            var cube = creaturesFactory.Create<CubeBehaviour>(spawnPosition);
            Spawn(cube.gameObject);
        }
    }
}