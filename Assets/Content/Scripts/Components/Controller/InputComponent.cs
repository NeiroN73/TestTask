using GameCore.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Components
{
    public class InputComponent : ControllerComponent
    {
        private PlayerInputActions _playerInputActions;
        
        public override Vector3 MoveDirection { get; protected set; }
        
        private readonly Subject _debugPerformed = new();
        private readonly Subject _spawnPerformed = new();
        public override IObservable DebugPerformed => _debugPerformed;
        public override IObservable SpawnPerformed => _spawnPerformed;

        protected override void Initialize()
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
            if (!isLocalPlayer) 
                return;

            var value = obj.ReadValue<Vector2>();
            MoveDirection = new Vector3(value.x, 0, value.y);
        }

        private void SpawnOnPerformed(InputAction.CallbackContext obj)
        {
            if (!isLocalPlayer) 
                return;
            
            _spawnPerformed.OnNext();
        }

        private void DebugOnPerformed(InputAction.CallbackContext obj)
        {
            if (!isLocalPlayer) 
                return;
            
            _debugPerformed.OnNext();
        }
    }
}