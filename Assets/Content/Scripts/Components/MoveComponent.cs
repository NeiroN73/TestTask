using Game.Configs;
using GameCore.Services;
using UnityEngine;

namespace Game.Components
{
    public class MoveComponent : CreatureComponent, ITickable
    {
        private static readonly int Running = Animator.StringToHash("isRunning");
        
        private CharacterController _characterController;
        private ControllerComponent _controllerComponent;
        private Animator _animator;
        private MoveData _moveData;

        public void Init(CharacterController characterController, ControllerComponent controllerComponent,
            MoveData moveData, Animator animator)
        {
            _characterController = characterController;
            _controllerComponent = controllerComponent;
            _moveData = moveData;
            _animator = animator;
        }

        public void Tick(float deltaTime)
        {
            if(!isLocalPlayer)
                return;

            if (!Creature)
                return;
            
            var moveDirection = _controllerComponent.MoveDirection;
            
            if (moveDirection.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(
                    new Vector3(moveDirection.x, 0, moveDirection.z), 
                    Vector3.up
                );
                
                Creature.transform.rotation = Quaternion.Slerp(
                    Creature.transform.rotation,
                    targetRotation,
                    _moveData.RotateSpeed * Time.deltaTime
                );
            }
            
            moveDirection *= _moveData.MoveSpeed;
        
            _characterController.Move(moveDirection * Time.deltaTime);
            
            bool isMoving = moveDirection.magnitude > 0.1f;
            _animator.SetBool(Running, isMoving);
        }
    }
}