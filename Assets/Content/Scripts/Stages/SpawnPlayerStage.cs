using Cysharp.Threading.Tasks;
using Game.Components;
using Game.Creatures;
using GameCore.Factories;
using Mirror;
using VContainer;

namespace Game.Stages
{
    public class SpawnPlayerStage : NetworkInstallerStage
    {
        [Inject] private CreaturesFactory creaturesFactory;
        
        public async override UniTask Run()
        {
            NetworkServer.RegisterHandler<CreatePlayerMessage>(OnCreateCharacter);
            RequestPlayerSpawn("Bob");
            await UniTask.CompletedTask;
        }
        
        public void RequestPlayerSpawn(string playerName)
        {
            if (!NetworkClient.active)
            {
                return;
            }

            if (!NetworkClient.ready)
            {
                NetworkClient.Ready();
            }

            CreatePlayerMessage message = new()
            {
                Name = playerName
            };
            
            NetworkClient.Send(message);
        }
        
        private void OnCreateCharacter(NetworkConnectionToClient conn, CreatePlayerMessage message)
        {
            if (!conn.isReady)
            {
                return;
            }

            var player = creaturesFactory.Create<PlayerServerCreature>();
            NetworkServer.AddPlayerForConnection(conn, player.gameObject);
            player.GetCreatureComponentByType<ChangeNameComponent>().Init(message.Name);
        }

        private struct CreatePlayerMessage : NetworkMessage
        {
            public string Name;
        }
    }
}