using Game.Configs;
using Game.Creatures;
using UnityEngine;

namespace Game.Components
{
    public class MoveServerElement : NetworkElement, IServerTickable
    {
        private readonly Transform _transform;
        private readonly CharacterController _characterController;
        private readonly MoveData _moveData;
        private Vector3 _moveDirection;
        
        public Observer<bool, Vector3, Quaternion> Moved = new();
        
        public MoveServerElement(Transform transform, CharacterController characterController, MoveData moveData)
        {
            _transform = transform;
            _characterController = characterController;
            _moveData = moveData;
        }

        public void ServerTick(float deltaTime)
        {
            if (_moveDirection.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(
                    new Vector3(_moveDirection.x, 0, _moveDirection.z), 
                    Vector3.up
                );
            
                _transform.rotation = Quaternion.Slerp(
                    _transform.rotation,
                    targetRotation,
                    _moveData.RotateSpeed * Time.deltaTime
                );
            }
        
            _moveDirection *= _moveData.MoveSpeed;
            _characterController.Move(_moveDirection * Time.deltaTime);
            var isMoving = _moveDirection.magnitude > 0.1f;
            
            Moved.Publish(isMoving, _transform.position, _transform.rotation);
        }

        public void SetMoveDirection(Vector3 moveDirection)
        {
            _moveDirection = moveDirection;
        }
    }
}