using Content.Scripts.EventBus;
using Game.Creatures;
using Game.Events;
using UnityEngine;

namespace Game.Components
{
    public class MoveClientElement : ClientNetworkElement, IClientTickable
    {
        private readonly Transform _transform;
        private readonly Animator _animator;
        
        private readonly int _running = Animator.StringToHash("isRunning");
        
        private Vector3 _direction;
        private Vector3 _position;
        private Quaternion _rotation;

        public MoveClientElement(Transform transform, Animator animator)
        {
            _transform = transform;
            _animator = animator;
        }

        public override void InvokeSubscribes()
        {
            BehaviourEventBus.ClientSubscribe<MovedOnServerEvent>(UpdateVisualMove).AddDisposable(Disposable);
        }

        public void Tick(float deltaTime)
        {
            _transform.position = _position;
            _transform.rotation = _rotation;
        }
        
        public void UpdateVisualMove(MovedOnServerEvent e)
        {
            _position = e.Position;
            _rotation = e.Rotation;
        
            _animator.SetBool(_running, e.IsMoved);
        }
    }
}