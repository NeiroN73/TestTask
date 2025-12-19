using Game.Creatures;
using UnityEngine;

namespace Game.Components
{
    public class AIServerElement : ControllerElement, IServerTickable, IClientTickable
    {
        public void Tick(float deltaTime)
        {
            var moveDirection = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10));
            MoveInputed.Publish(moveDirection);
        }
    }
}