using Cysharp.Threading.Tasks;
using FishNet.Connection;
using FishNet.Object;
using Game.NetworkInterfaces;
using Game.Services;
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
            CreateServerRpc(_networkConnection);
            
            await UniTask.CompletedTask;
        }
        
        [ServerRpc(RequireOwnership = false)]
        private void CreateServerRpc(NetworkConnection conn)
        {
            //todo: сделать систему айдишников
            networkBehavioursFactory.Create("bob", networkConnection: conn);
        }
    }
}