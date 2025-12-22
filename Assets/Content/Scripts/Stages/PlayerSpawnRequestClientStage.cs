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
        [Inject] private NetworkBehavioursClientFactory _networkBehavioursClientFactory;
        
        private NetworkConnection _networkConnection;

        public void Initialize(NetworkConnection networkConnection)
        {
            _networkConnection = networkConnection;
        }
        
        public override async UniTask Run()
        {
            _networkBehavioursClientFactory.RequestCreateAsync("bob", _networkConnection, rotation: Quaternion.identity);

            await UniTask.CompletedTask;
        }
    }
}