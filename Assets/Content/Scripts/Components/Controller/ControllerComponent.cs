using Game.Observers;
using UnityEngine;

namespace Game.Components
{
    public abstract class ControllerComponent : NetworkComponent
    {
        public Observer<Vector3> MovePerformed = new();
        public Observer DebugPerformed { get; } = new();
        public Observer SpawnPerformed { get; } = new();
    }
}