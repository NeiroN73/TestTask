using Game.Creatures;
using R3;
using UnityEngine;

namespace Game.Components
{
    public class MoveClientComponent : ClientNetworkComponent, 
        IClientPreInitializable, IClientTickable
    {
        private Animator _animator;
        private readonly int _running = Animator.StringToHash("isRunning");
        
        private Vector3 _targetPosition;
        private Quaternion _targetRotation;
        
        private float _moveSpeed = 10f;
        private float _rotateSpeed = 10f;
        
        public void Configure(Animator animator)
        {
            _animator = animator;
        }

        public void PreInitialize()
        {
            if (ComponentsContainer.TryGetNetworkComponent<MoveServerComponent>(out var moveServerComponent))
            {
                moveServerComponent.OnMoved.Subscribe(UpdateVisualMove).AddTo(Disposable);
            }
        }

        public void Tick(float deltaTime)
        {
            transform.position = _targetPosition;
            transform.rotation = _targetRotation;
        }
        
        public void UpdateVisualMove(Vector3 position, Quaternion rotation, bool isMoving)
        {
            _targetPosition = position;
            _targetRotation = rotation;
            
            Debug.Log("Client tick " + position);
            
            _animator.SetBool(_running, isMoving);
        }
    }
}