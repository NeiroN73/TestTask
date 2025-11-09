using System.Collections.Generic;
using Game.Components;
using Game.NetworkManagers;
using GameCore.Creatures;
using Mirror;
using UnityEngine;
using VContainer;

namespace Game.Creatures
{
    public abstract class NetworkCreature : NetworkBehaviour, ICreature
    {
        [field: SerializeField] public string Id { get; private set; }
        
        protected IObjectResolver ClientObjectResolver { get; private set; }
        protected IObjectResolver ServerObjectResolver { get; private set; }
        
        protected List<CreatureComponent> Components = new();
        
        public override void OnStartClient()
        {
            if (NetworkManager.singleton is GameNetworkManager manager) // костыльный инжект, ничего лучше не придумать?
            {
                //ClientObjectResolver = manager.ObjectResolver;
                ClientObjectResolver.Inject(this);
            }
        }

        public void Inject()
        {
            
        }
        
        public abstract void Initialize();
    }

    public abstract class ClientNetworkCreature : NetworkCreature
    {
        public override void Initialize()
        {
            
        }

        public void ClientInitialize()
        {
            
        }
    }

    public abstract class ServerNetworkCreature : NetworkCreature
    {
        public override void Initialize()
        {
            
        }

        public void ServerInitialize()
        {
            
        }
    }
}