using FishNet.Connection;
using Game.Creatures;
using Game.Events;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Components
{
    public class InputLocalClientElement : ControllerElement, ILocalClientInitializable
    {
        private readonly NetworkConnection _networkConnection;
        private PlayerInputActions _playerInputActions;

        public InputLocalClientElement(NetworkConnection networkConnection)
        {
            _networkConnection = networkConnection;
        }
        
        public void Initialize()
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
            BehaviourEventBus.PublishServerRpc(new MoveInputedEvent
            {
                NetworkConnection = _networkConnection,
                Direction = moveDirection
            });
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