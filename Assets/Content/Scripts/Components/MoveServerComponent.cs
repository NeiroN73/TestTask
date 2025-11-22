using Game.Configs;
using Game.Creatures;
using Mirror;
using R3;
using UnityEngine;

namespace Game.Components
{
    public class MoveServerComponent : NetworkElement, IServerTickable
    {
        private readonly Transform _transform;
        private readonly ControllerComponentParent controllerComponentParent;
        private readonly CharacterController _characterController;
        private readonly MoveData _moveData;

        public Observer<bool, Vector3, Quaternion> Moved = new();
        
        public MoveServerComponent(Transform transform, ControllerComponentParent controllerComponentParent,
            CharacterController characterController, MoveData moveData)
        {
            _transform = transform;
            this.controllerComponentParent = controllerComponentParent;
            _characterController = characterController;
            _moveData = moveData;
        }

        public void ServerTick(float deltaTime)
        {
            var moveDirection = controllerComponentParent.MoveDirection;
        
            if (moveDirection.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(
                    new Vector3(moveDirection.x, 0, moveDirection.z), 
                    Vector3.up
                );
            
                _transform.rotation = Quaternion.Slerp(
                    _transform.rotation,
                    targetRotation,
                    _moveData.RotateSpeed * Time.deltaTime
                );
            }
        
            moveDirection *= _moveData.MoveSpeed;
    
            _characterController.Move(moveDirection * Time.deltaTime);
        
            bool isMoving = moveDirection.magnitude > 0.1f;
            
            Moved.Publish(isMoving, _transform.position, _transform.rotation);
        }
    }
    
    public class MoveClientComponent : NetworkElement, IClientTickable
    {
        private readonly Transform _transform;
        private readonly CharacterController _characterController;
        private readonly Animator _animator;
        
        private readonly int _running = Animator.StringToHash("isRunning");
        private Vector3 _direction;

        public MoveClientComponent(Transform transform, CharacterController characterController, Animator animator)
        {
            _transform = transform;
            _characterController = characterController;
            _animator = animator;
        }

        public void UpdateVisualMove(bool isMoving, Vector3 position, Quaternion rotation)
        {
            _transform.position = position;
            _transform.rotation = rotation;
        
            _animator.SetBool(_running, isMoving);
        }

        public void ClientTick(float deltaTime)
        {
            _characterController.Move(_direction * deltaTime);
            
            var isMoving = _direction.magnitude > 0.1f;
            _animator.SetBool(_running, isMoving);
        }
    }

    public class MoveComponentParent : NetworkParent
    {
        public MoveServerComponent Server { get; set; }
        public MoveClientComponent Client { get; set; }
        
        public void Initialize()
        {
            Server.Moved.Subscribe(MoveClientRpc).AddTo(Disposable);
        }
        
        [ClientRpc]
        public void MoveClientRpc(bool isMoving, Vector3 position, Quaternion rotation)
        {
            Client.UpdateVisualMove(isMoving, position, rotation);
        }
    }
}