using Game.NetworkInterfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Components
{
    public class InputLocalClientComponent : ControllerComponent, ILocalClientInitializable, ILocalClientDisposable
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
            MovePerformed.Publish(moveDirection);
        }
        
        private void SpawnOnPerformed(InputAction.CallbackContext obj)
        {
            SpawnPerformed.Publish();
        }

        private void DebugOnPerformed(InputAction.CallbackContext obj)
        {
            DebugPerformed.Publish();
        }

        public void LocalClientDispose()
        {
            if (_playerInputActions != null)
            {
                _playerInputActions.Defaultactionmap.Debug.performed -= DebugOnPerformed;
                _playerInputActions.Defaultactionmap.Spawn.performed -= SpawnOnPerformed;
                _playerInputActions.Defaultactionmap.Move.performed -= MoveOnPerformed;
                _playerInputActions.Defaultactionmap.Move.canceled -= MoveOnPerformed;
                
                _playerInputActions.Disable();
                
                _playerInputActions.Dispose();
                _playerInputActions = null;
            }
        }
    }
}