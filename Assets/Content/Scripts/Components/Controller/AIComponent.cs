using Game.Creatures;
using UnityEngine;

namespace Game.Components
{
    public class AIComponent : ControllerComponentParent
    {
        public AIServerComponent Server { get; set; }
        
        public override Vector3 MoveDirection => Server.MoveDirection;
        public override Observer DebugPerformed { get; } = new();
        public override Observer SpawnPerformed { get; } = new();
    }

    public class AIServerComponent : IServerTickable
    {
        public Vector3 MoveDirection { get; private set; }

        public void ServerTick(float deltaTime)
        {
            MoveDirection = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10));
        }
    }
}