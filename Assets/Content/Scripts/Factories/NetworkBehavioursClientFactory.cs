using FishNet.Connection;
using FishNet.Object;
using Game.Creatures;
using UnityEngine;

namespace Content.Scripts.Factories
{
    public class NetworkBehavioursClientFactory : NetworkFactory, IClientInjectable
    {
        public void RequestCreateAsync(
            string id, 
            NetworkConnection networkConnection,
            Vector3 position = default, 
            Quaternion rotation = default, 
            Transform parent = null)
        {
            var f = FindAnyObjectByType<NetworkBehavioursServerFactory>();
                f.Initialize();
                f.Create(id, networkConnection, position, rotation, parent);
        }
    }
}