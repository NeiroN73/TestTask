using GameCore.Utils;
using UnityEngine;

namespace Game.Components
{
    public abstract class ControllerComponentParent : NetworkParent
    {
        public abstract Vector3 MoveDirection { get; }
        public abstract Observer DebugPerformed { get; }
        public abstract Observer SpawnPerformed { get; }
    }
}