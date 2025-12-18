using FishNet.Object;
using Game.Components;
using Game.Configs;
using R3;
using UnityEngine;
using VContainer;

namespace Game.Creatures
{
    public class PlayerBehaviour : BaseNetworkBehaviour, IClientsInjectable
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private CharacterController _characterController;
        
        [Inject] private PlayerConfig _playerConfig;
        
        private MoveServerElement _moveServerElement;
        private MoveClientElement _moveClientElement;
        private ControllerElement _controllerElement;
        
        public override void Initialize()
        {
            _moveServerElement = new MoveServerElement(transform, _characterController, _playerConfig.MoveData);
            _moveClientElement = new MoveClientElement(transform, _characterController, _animator);
            _controllerElement = new InputLocalClientElement();
            
            AddElements(_moveServerElement, _moveClientElement, _controllerElement);
            
            _moveServerElement.Moved.Subscribe(OnMovedObserversRpc).AddTo(Disposable);
            _controllerElement.MoveInputed.Subscribe(OnMoveInputedServerRpc).AddTo(Disposable);
        }
        
        [ObserversRpc]
        private void OnMovedObserversRpc(bool isMoving, Vector3 position, Quaternion rotation)
        {
            _moveClientElement.UpdateVisualMove(isMoving, position, rotation);
        }

        [ServerRpc]
        private void OnMoveInputedServerRpc(Vector3 direction)
        {
            _moveServerElement.SetMoveDirection(direction);
        }
    }
}