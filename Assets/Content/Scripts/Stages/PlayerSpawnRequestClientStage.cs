using Content.Scripts.Factories;
using Cysharp.Threading.Tasks;
using FishNet.Connection;
using FishNet.Object;
using Game.Creatures;
using VContainer;

namespace Game.Stages
{
    public class PlayerSpawnRequestClientStage : NetworkInstallerStage, IInjectable
    {
        [Inject] private NetworkBehavioursFactory networkBehavioursFactory;
        
        private NetworkConnection _networkConnection;
        
        public void Configure(NetworkConnection networkConnection)
        {
            _networkConnection = networkConnection;
        }
        
        public override async UniTask Run()
        {
            CreateServerRpc();
            
            await UniTask.CompletedTask;
        }
        
        [ServerRpc(RequireOwnership = false)]
        private void CreateServerRpc()
        {
            networkBehavioursFactory.Create("bob", networkConnection: _networkConnection);
        }
    }
}