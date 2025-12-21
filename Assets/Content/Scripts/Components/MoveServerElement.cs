using Content.Scripts.EventBus;
using FishNet.Connection;
using Game.Configs;
using Game.Creatures;
using Game.Events;
using UnityEngine;

namespace Game.Components
{
    public class MoveServerElement : ServerNetworkElement, IServerTickable
    {
        private NetworkConnection _networkConnection;
        private readonly Transform _transform;
        private readonly CharacterController _characterController;
        private readonly MoveData _moveData;
        private Vector3 _moveDirection;
        
        public MoveServerElement(Transform transform, CharacterController characterController, MoveData moveData)
        {
            _transform = transform;
            _characterController = characterController;
            _moveData = moveData;
        }

        public override void InvokeSubscribes()
        {
            BehaviourEventBus.ServerSubscribe<MoveInputedEvent>(OnSettedMoveDirection).AddDisposable(Disposable);
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
        
                    _transform.rotation = Quaternion.Slerp(
                        _transform.rotation,
                        targetRotation,
                        _moveData.RotateSpeed * deltaTime
                    );
                }
            }
    
            Vector3 velocity = _moveDirection * _moveData.MoveSpeed;
            _characterController.Move(velocity * deltaTime);
    
            var isMoving = _moveDirection.magnitude > 0.1f;
            
            BehaviourEventBus.PublishTargetRpc(_networkConnection, new MovedOnServerEvent
            {
                IsMoved = isMoving,
                Position = _transform.position,
                Rotation = _transform.rotation
            });
        }
        
        public void OnSettedMoveDirection(MoveInputedEvent e)
        {
            _networkConnection = e.NetworkConnection;
            _moveDirection = e.Direction;
        }
    }
}