using FishNet.Connection;
using FishNet.Object;
using Game.Configs;
using Game.Creatures;
using R3;
using UnityEngine;
using VContainer;

namespace Game.Components
{
    public class MoveComponent : ServerNetworkComponent,
        IInjectable,
        IClientPreInitializable,
        IServerTickable,
        IClientTickable
    {
        private readonly int _running = Animator.StringToHash("isRunning");
        
        private NetworkConnection _networkConnection;
        private CharacterController _characterController;
        private Animator _animator;
        private Vector3 _moveDirection;
        
        private InputLocalClientComponent _inputLocalClientComponent;
        
        [Inject] private PlayerConfig _playerConfig;

        public void Configure(CharacterController characterController, Animator animator)
        {
            _characterController = characterController;
            _animator = animator;
        }
        
        public void ClientPreInitialize()
        {
            if (ComponentsContainer.TryGetNetworkComponent<ControllerComponent>(out var component))
            {
                component.MoveInputed.Subscribe(OnSettedMoveDirection).AddTo(Disposable);
            }
        }

        public void ServerTick(float deltaTime)
        {
            Move(deltaTime);
        }

        private void OnSettedMoveDirection(Vector3 direction)
        {
            _moveDirection = direction;

            OnServerSettedMoveDirection(direction);
        }
        
        [ServerRpc]
        private void OnServerSettedMoveDirection(Vector3 direction)
        {
            _moveDirection = direction;
        }
        
        private void Move(float deltaTime)
        {
            if (_moveDirection.magnitude > 0.1f)
            {
                Vector3 horizontalDirection = new Vector3(_moveDirection.x, 0, _moveDirection.z);
        
                if (horizontalDirection.sqrMagnitude > 0.001f)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(
                        horizontalDirection.normalized, 
                        Vector3.up
                    );
        
                    transform.rotation = Quaternion.Slerp(
                        transform.rotation,
                        targetRotation,
                        _playerConfig.MoveData.RotateSpeed * deltaTime
                    );
                }
            }
    
            Vector3 velocity = _moveDirection * _playerConfig.MoveData.MoveSpeed;
            _characterController?.Move(velocity * deltaTime);
    
            var isMoving = _moveDirection.magnitude > 0.1f;
            _animator.SetBool(_running, isMoving);   
        }

        public void ClientTick(float deltaTime)
        {
            Move(deltaTime);
        }
    }
}