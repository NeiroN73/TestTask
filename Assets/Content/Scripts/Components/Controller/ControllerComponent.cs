using GameCore.ReactiveObservers;
using UnityEngine;

namespace Game.Components
{
    public abstract class ControllerComponent : NetworkComponent
    {
        public ReactiveObserver<Vector3> MovePerformed = new();
        public ReactiveObserver DebugPerformed { get; } = new();
        public ReactiveObserver SpawnPerformed { get; } = new();
    }
}