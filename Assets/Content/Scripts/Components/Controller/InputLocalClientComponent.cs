using FishNet.Connection;
using FishNet.Object;
using Game.Creatures;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Components
{
    public class InputLocalClientComponent : ControllerComponent, ILocalClientInitializable
    {
        private PlayerInputActions _playerInputActions;
        
        public void LocalClientInitialize()
        {
            _playerInputActions = new();
            _playerInputActions.Enable();
            
            _playerInputActions.Defaultactionmap.Debug.performed += DebugOnPerformed;
            _playerInputActions.Defaultactionmap.Spawn.performed += SpawnOnPerformed;
            _playerInputActions.Defaultactionmap.Move.performed += MoveOnPerformed;
            _playerInputActions.Defaultactionmap.Move.canceled += MoveOnPerformed;
        }
        
        private void MoveOnPerformed(InputAction.CallbackContext obj)
        {
            var value = obj.ReadValue<Vector2>();
            var moveDirection = new Vector3(value.x, 0, value.y);
            
            MoveTarget(moveDirection);
        }

        private void MoveTarget(Vector3 direction)
        {
            MoveInputed.Publish(direction);
        }
        
        private void SpawnOnPerformed(InputAction.CallbackContext obj)
        {
            SpawnPerformed.Publish();
        }

        private void DebugOnPerformed(InputAction.CallbackContext obj)
        {
            DebugPerformed.Publish();
        }
    }
}