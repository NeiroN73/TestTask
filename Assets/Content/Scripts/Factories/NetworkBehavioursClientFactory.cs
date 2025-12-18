using FishNet;
using FishNet.Connection;
using GameCore.Creatures;
using GameCore.Factories;
using UnityEngine;

namespace Content.Scripts.Factories
{
    public class NetworkBehavioursClientFactory : Factory
    {
        public TCreature RequestCreate<TCreature>(string id, NetworkConnection networkConnection, Vector3 position = default, Quaternion rotation = default, Transform parent = null) 
            where TCreature : MonoBehaviour, ICreature
        {
            var spawnArgs = new BehaviourSpawnArgs
            {
                Id = id,
                NetworkConnection = networkConnection,
                Position = position,
                Rotation = rotation,
                Parent = parent
            };
            
            InstanceFinder.ServerManager.Broadcast(networkConnection, spawnArgs);

            return null;//Create(prefab, position, rotation, parent);
        }
    }
}