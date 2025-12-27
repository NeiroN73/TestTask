using FishNet.Object;
using Game.Configs;
using Game.NetworkInterfaces;
using R3;
using UnityEngine;
using VContainer;

namespace Game.Components
{
    public class MoveComponent : NetworkComponent, IInjectable, IClientPreInitializable, IServerTickable,
        IClientTickable
    {
        private const float _lerpValue = 0.1f;
        private readonly int _isRunning = Animator.StringToHash("isRunning");
        
        private CharacterController _characterController;
        private Animator _animator;
        
        private InputLocalClientComponent _inputLocalClientComponent;
        
        [Inject] private PlayerConfig _playerConfig;
        
        private Vector3 _moveDirection;
        private Vector3 _movePosition;
        private Quaternion _moveRotation;
        private bool _isMoving;
        
        public void Configure(CharacterController characterController, Animator animator)
        {
            _characterController = characterController;
            _animator = animator;
        }
        
        public void ClientPreInitialize()
        {
            if (ComponentsContainer.TryGetNetworkComponent<ControllerComponent>(out var component))
            {
                component.MovePerformed.Subscribe(OnMovedDirectionServerRpc).AddTo(Disposable);
            }
        }

        public void ServerTick(float deltaTime)
        {
            if (_moveDirection.magnitude > 0.1f)
            {
                var horizontalDirection = new Vector3(_moveDirection.x, 0, _moveDirection.z);
                var targetRotation = Quaternion.LookRotation(
                    horizontalDirection.normalized, Vector3.up);
    
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation,
                    _playerConfig.MoveData.RotateSpeed * deltaTime
                );
            }
    
            var velocity = _moveDirection * _playerConfig.MoveData.MoveSpeed;
            _characterController.Move(velocity * deltaTime);
            var isMoving = _moveDirection.magnitude > 0.1f;
           
            UpdateMovementObserversRpc(transform.position, transform.rotation, isMoving);
        }
        
        //todo: сделать через prediction
        public void ClientTick(float deltaTime)
        {
            transform.position = Vector3.LerpUnclamped(transform.position, _movePosition, _lerpValue);
            transform.rotation = Quaternion.SlerpUnclamped(transform.rotation, _moveRotation, _lerpValue);
            _animator.SetBool(_isRunning, _isMoving);
        }

        [ServerRpc]
        private void OnMovedDirectionServerRpc(Vector3 direction)
        {
            _moveDirection = direction;
        }
        
        [ObserversRpc]
        private void UpdateMovementObserversRpc(Vector3 movePosition, Quaternion moveRotation, bool isMoving)
        {
            _movePosition = movePosition;
            _moveRotation = moveRotation;
            _isMoving = isMoving;
        }
    }
}