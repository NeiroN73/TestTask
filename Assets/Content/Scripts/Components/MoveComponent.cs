using Game.Configs;
using Game.Creatures;
using GameCore.Services;
using UnityEngine;

namespace Game.Components
{
    public class MoveComponent : CreatureComponent, IServerTag, ITickable
    {
        private static readonly int Running = Animator.StringToHash("isRunning");
        
        private CharacterController _characterController;
        private Animator _animator;
        private MoveData _moveData;
        
        private ControllerComponent _controllerComponent;

        public void Initialize(CharacterController characterController,
            MoveData moveData, Animator animator)
        {
            _characterController = characterController;
            _moveData = moveData;
            _animator = animator;
            
            _controllerComponent = Creature.GetCreatureComponentByType<ControllerComponent>();
        }

        public void Tick(float deltaTime)
        {
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