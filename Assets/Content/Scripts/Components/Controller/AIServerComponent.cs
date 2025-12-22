using Game.Creatures;
using Game.Events;
using UnityEngine;

namespace Game.Components
{
    public class AIServerComponent : ControllerComponent, IServerTickable, IClientTickable
    {
        public void Tick(float deltaTime)
        {
            var moveDirection = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10));
        }
    }
}