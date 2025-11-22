using System;
using Cysharp.Threading.Tasks;
using Game.Components;
using Game.Creatures;
using Game.NetworkManagers;
using GameCore.Factories;
using Mirror;
using R3;
using VContainer;

namespace Game.Stages
{
    public class SpawnPlayerStage : NetworkInstallerStage
    {
        [Inject] private CreaturesFactory _creaturesFactory;
        [Inject] private GameNetworkManager _gameNetworkManager;
        
        private bool _stageCompleted;

        public override void ServerInitialize()
        {
            _gameNetworkManager.ClientConnected.Subscribe(OnPlayerSpawned).AddTo(Disposables);
        }

        public async override UniTask ServerRun()
        {
            await UniTask.WaitUntil(() => _stageCompleted);
        }

        private void OnPlayerSpawned(NetworkConnectionToClient conn)
        {
            SpawnPlayerCommand(conn, "bob");
        }
        
        private void SpawnPlayerCommand(NetworkConnectionToClient conn, string playerName)
        {
            var player = _creaturesFactory.Create<PlayerClientEntity>();
            NetworkServer.AddPlayerForConnection(conn, player.gameObject);
            player.GetEntityComponentByType<ChangeNameComponent>().Init(playerName);

            _stageCompleted = true;
        }
    }
}