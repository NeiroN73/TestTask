using System;
using Cysharp.Threading.Tasks;
using Game.Components;
using Game.Creatures;
using GameCore.Factories;
using Mirror;
using R3;
using VContainer;

namespace Game.Stages
{
    public class SpawnPlayerStage : NetworkInstallerStage
    {
        [Inject] private CreaturesFactory creaturesFactory;

        private readonly Subject<NetworkIdentity> _playerSpawned = new();
        private readonly Subject<NetworkIdentity> _playerDespawned = new();
        public Observable<NetworkIdentity> PlayerSpawned => _playerSpawned;
        public Observable<NetworkIdentity> PlayerDespawned => _playerDespawned;
        
        public async override UniTask Run()
        {
            if (NetworkClient.active)
            {
                SpawnPlayerCommand(NetworkClient.localPlayer.connectionToClient, "bob");
            }
            await UniTask.CompletedTask;
        }
        
        [Command(requiresAuthority = false)]
        private void SpawnPlayerCommand(NetworkConnectionToClient conn, string playerName)
        {
            var player = creaturesFactory.Create<PlayerEntity>();
            NetworkServer.AddPlayerForConnection(conn, player.gameObject);
            player.GetEntityComponentByType<ChangeNameComponent>().Init(playerName);
            var networkIdentity = player.GetComponent<NetworkIdentity>();
            _playerSpawned.OnNext(networkIdentity);
        }
    }
}