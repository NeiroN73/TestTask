using UnityEngine;

namespace Game.Components
{
    public abstract class ControllerElement : NetworkElement
    {
        public Observer<Vector3> MoveInputed { get; } = new();
        public Observer DebugPerformed { get; } = new();
        public Observer SpawnPerformed { get; } = new();
    }
}