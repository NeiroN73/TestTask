using Game.Creatures;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Components
{
    public class InputComponent : ControllerComponentParent
    {
        public InputLocalClientComponent LocalClient { get; set; }

        public override Vector3 MoveDirection => LocalClient.MoveDirection;
        public override Observer DebugPerformed => LocalClient.DebugPerformed;
        public override Observer SpawnPerformed => LocalClient.SpawnPerformed;
    }
    
    public class InputLocalClientComponent : ILocalClientInitializable
    {
        private PlayerInputActions _playerInputActions;
        
        public Vector3 MoveDirection { get; private set; }
        public Observer DebugPerformed { get; } = new();
        public Observer SpawnPerformed { get; } = new();
        
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
            MoveDirection = new Vector3(value.x, 0, value.y);
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