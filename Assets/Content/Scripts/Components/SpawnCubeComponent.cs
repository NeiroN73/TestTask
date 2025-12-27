using FishNet.Object;
using Game.NetworkInterfaces;
using Game.Services;
using R3;
using VContainer;

namespace Game.Components
{
    public class SpawnCubeComponent : NetworkComponent, IInjectable, IClientPreInitializable
    {
        [Inject] private NetworkBehavioursFactory _networkBehavioursFactory;

        private string _cubeId;

        public void Configure(string cubeId)
        {
            _cubeId = cubeId;
        }
        
        public void ClientPreInitialize()
        {
            if (ComponentsContainer.TryGetNetworkComponent<ControllerComponent>(out var controllerComponent))
            {
                controllerComponent.SpawnPerformed.Subscribe(SpawnCubeServerRpc).AddTo(Disposable);
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void SpawnCubeServerRpc()
        {
            var spawnPosition = transform.position + transform.forward;
            _networkBehavioursFactory.Create(_cubeId, position: spawnPosition);
        }
    }
}