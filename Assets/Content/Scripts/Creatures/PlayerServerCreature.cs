using Game.Components;
using Game.Configs;
using GameCore.Factories;
using GameCore.Services;
using Mirror;
using UnityEngine;
using VContainer;

namespace Game.Creatures
{
    public class PlayerServerCreature : ServerNetworkCreature
    {
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] public CharacterController CharacterController { get; private set; }
        
        [field: SerializeField] public ControllerComponent ControllerComponent { get; private set; }
        [field: SerializeField] public MoveComponent MoveComponent { get; private set; }
        [field: SerializeField] public SpawnComponent SpawnComponent { get; private set; }
        [field: SerializeField] public SendDebugComponent SendDebugComponent { get; private set; }
        [field: SerializeField] public ChangeNameComponent ChangeNameComponent { get; private set; }
        
        [Inject] private PlayerConfig _playerConfig;
        [Inject] private TickService _tickService;
        [Inject] private ViewsFactory _viewsFactory;
        [Inject] private CreaturesFactory creaturesFactory;
        
        [TargetRpc]
        public void TargetRpcInitialize(NetworkConnection conn)
        {
            Components = new()
            {
                ControllerComponent,
                MoveComponent,
                SpawnComponent,
                ChangeNameComponent,
                SendDebugComponent
            };
            
            foreach (var component in Components)
            {
                component.Init(this);
            }
            
            ControllerComponent.Init();
            MoveComponent.Init(CharacterController, ControllerComponent, _playerConfig.MoveData, Animator);
            SpawnComponent.Init(ControllerComponent);
            SendDebugComponent.Init(ControllerComponent, ChangeNameComponent);
            
            _tickService.RegisterTick(MoveComponent);
        }

        [ClientRpc]
        public void RpcSetName(string newName)
        {
            ChangeNameComponent.Init(newName);
        }

        public override void OnStopClient()
        {
            base.OnStopClient();
            
            foreach (var component in Components)
            {
                component?.Dispose();
            }
        }
    }
    
    public class PlayerClientCreature : ClientNetworkCreature
    {
        
    }
}