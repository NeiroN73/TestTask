using System;
using FishNet.Connection;
using FishNet.Object;
using Game.Configs;
using Game.Creatures;
using R3;
using UnityEngine;
using VContainer;

namespace Game.Components
{
    public class MoveServerComponent : ServerNetworkComponent, IServerInjectable, IServerPreInitializable, IServerTickable
    {
        private NetworkConnection _networkConnection;
        private CharacterController _characterController;
        private Vector3 _moveDirection;
        
        private InputLocalClientComponent _inputLocalClientComponent;
        
        [Inject] private PlayerConfig _playerConfig;

        public Observer<Vector3, Quaternion, bool> OnMoved = new();
        
        public void Configure(CharacterController characterController)
        {
            _characterController = characterController;
        }
        
        public void PreInitialize()
        {
            if (ComponentsContainer.TryGetNetworkComponent<ControllerComponent>(out var component))
            {
                component.MoveInputed.Subscribe(OnSettedMoveDirection).AddTo(Disposable);
            }
        }

        public void Tick(float deltaTime)
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
            MoveOnClients(transform.position, transform.rotation, isMoving);
        }

        private void OnSettedMoveDirection(Vector3 direction)
        {
            _moveDirection = direction;
        }

        [ObserversRpc]
        private void MoveOnClients(Vector3 position, Quaternion rotation, bool isMoving)
        {
            OnMoved.Publish(position, rotation, isMoving);
            
            Debug.Log("Server tick " + position);
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 0.5f);
        }
    }
}