using Cysharp.Threading.Tasks;
using FishNet.Connection;
using Game.Creatures;
using GameCore.Factories;
using VContainer;

namespace Game.Stages
{
    public class SpawnPlayerStage : NetworkInstallerStage
    {
        [Inject] private CreaturesFactory _creaturesFactory;
        
        private bool _stageCompleted;

        public override void ServerInitialize()
        {
            //_gameNetworkManager.ClientConnected.Subscribe(OnPlayerSpawned).AddTo(Disposables);
        }

        public async override UniTask ServerRun()
        {
            await UniTask.WaitUntil(() => _stageCompleted);
        }

        private void OnPlayerSpawned(NetworkConnection conn)
        {
            SpawnPlayerCommand(conn, "bob");
        }
        
        private void SpawnPlayerCommand(NetworkConnection conn, string playerName)
        {
            var player = _creaturesFactory.Create<PlayerBehaviour>();
            NetworkManager.ServerManager.Spawn(player.gameObject, conn);

            _stageCompleted = true;
        }
    }
}