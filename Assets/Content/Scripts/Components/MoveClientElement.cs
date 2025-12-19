using Game.Creatures;
using UnityEngine;

namespace Game.Components
{
    public class MoveClientElement : NetworkElement, IClientTickable
    {
        private readonly Transform _transform;
        private readonly CharacterController _characterController;
        private readonly Animator _animator;
        
        private readonly int _running = Animator.StringToHash("isRunning");
        
        private Vector3 _direction;
        private Vector3 _position;
        private Quaternion _rotation;

        public MoveClientElement(Transform transform, CharacterController characterController, Animator animator)
        {
            _transform = transform;
            _characterController = characterController;
            _animator = animator;
        }

        public void Tick(float deltaTime)
        {
            _transform.position = _position;
            _transform.rotation = _rotation;
            
            _characterController.Move(_direction * deltaTime);
            
            var isMoving = _direction.magnitude > 0.1f;
            _animator.SetBool(_running, isMoving);
        }
        
        public void UpdateVisualMove(bool isMoving, Vector3 position, Quaternion rotation)
        {
            _position = position;
            _rotation = rotation;
        
            _animator.SetBool(_running, isMoving);
        }
    }
}