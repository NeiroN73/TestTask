using UnityEngine;

namespace Game.Components
{
    public abstract class ControllerComponent : NetworkComponent
    {
        public Observer<Vector3> MoveInputed = new();
        public Observer DebugPerformed { get; } = new();
        public Observer SpawnPerformed { get; } = new();
    }
}