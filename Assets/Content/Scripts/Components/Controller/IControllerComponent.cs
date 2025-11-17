using GameCore.Utils;
using UnityEngine;

namespace Game.Components
{
    public abstract class ControllerComponent : EntityComponent
    {
        public abstract Vector3 MoveDirection { get; protected set; }
        public abstract IObservable DebugPerformed { get; }
        public abstract IObservable SpawnPerformed { get; }
    } 
}