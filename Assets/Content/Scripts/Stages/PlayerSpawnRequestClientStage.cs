using Content.Scripts.Factories;
using Cysharp.Threading.Tasks;
using FishNet.Connection;
using Game.Creatures;
using UnityEngine;
using VContainer;

namespace Game.Stages
{
    public class PlayerSpawnRequestClientStage : NetworkInstallerStage, IClientInjectable
    {
        private readonly NetworkConnection _networkConnection;
        
        [Inject] private NetworkBehavioursClientFactory _networkBehavioursClientFactory;

        public PlayerSpawnRequestClientStage(NetworkConnection networkConnection)
        {
            _networkConnection = networkConnection;
        }
        
        public override async UniTask Run()
        {
            var player = await _networkBehavioursClientFactory.RequestCreateAsync<PlayerBehaviour>("bob",
                _networkConnection, rotation: Quaternion.identity);
        }
    }
}