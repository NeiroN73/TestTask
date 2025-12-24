using System.Collections.Generic;
using Content.Scripts.Factories;
using Game.Creatures;
using GameCore.Services;

namespace Game.Services
{
    public class NetworkTickService : NetworkService
    {
        private List<IClientPreTickable> _clientPreTickables = new();
        private List<IClientTickable> _clientTickables = new();
        private List<IClientPostTickable> _clientPostTickables = new();
        private List<IServerPreTickable> _serverPreTickables = new();
        private List<IServerTickable> _serverTickables = new();
        private List<IServerPostTickable> _serverPostTickables = new();
        private List<ILocalClientPreTickable> _localClientPreTickables = new();
        private List<ILocalClientTickable> _localClientTickables = new();
        private List<ILocalClientPostTickable> _localClientPostTickables = new();
        private List<IOtherClientsPreTickable> _otherClientsPreTickables = new();
        private List<IOtherClientsTickable> _otherClientsTickables = new();
        private List<IOtherClientsPostTickable> _otherClientsPostTickables = new();
        
        public void RegisterClientPreTick(IClientPreTickable tickable)
        {
            _clientPreTickables.Add(tickable);
        }

        public void RegisterClientTick(IClientTickable tickable)
        {
            _clientTickables.Add(tickable);
        }

        public void RegisterClientPostTick(IClientPostTickable tickable)
        {
            _clientPostTickables.Add(tickable);
        }

        public void RegisterServerPreTick(IServerPreTickable tickable)
        {
            _serverPreTickables.Add(tickable);
        }

        public void RegisterServerTick(IServerTickable tickable)
        {
            _serverTickables.Add(tickable);
        }

        public void RegisterServerPostTick(IServerPostTickable tickable)
        {
            _serverPostTickables.Add(tickable);
        }

        public void RegisterLocalClientPreTick(ILocalClientPreTickable tickable)
        {
            _localClientPreTickables.Add(tickable);
        }

        public void RegisterLocalClientTick(ILocalClientTickable tickable)
        {
            _localClientTickables.Add(tickable);
        }

        public void RegisterLocalClientPostTick(ILocalClientPostTickable tickable)
        {
            _localClientPostTickables.Add(tickable);
        }

        public void RegisterOtherClientsPreTick(IOtherClientsPreTickable tickable)
        {
            _otherClientsPreTickables.Add(tickable);
        }

        public void RegisterOtherClientsTick(IOtherClientsTickable tickable)
        {
            _otherClientsTickables.Add(tickable);
        }

        public void RegisterOtherClientsPostTick(IOtherClientsPostTickable tickable)
        {
            _otherClientsPostTickables.Add(tickable);
        }

        public void ClientTick(float deltaTime)
        {
            for (int i = 0; i < _clientTickables.Count; i++)
            {
                if (_clientTickables[i] == null)
                {
                    _clientTickables.RemoveAt(i);
                    continue;
                }

                _clientTickables[i].ClientTick(deltaTime);
            }
        }

        public void ServerTick(float deltaTime)
        {
            for (int i = 0; i < _serverTickables.Count; i++)
            {
                if (_serverTickables[i] == null)
                {
                    _serverTickables.RemoveAt(i);
                    continue;
                }

                _serverTickables[i].ServerTick(deltaTime);
            }
        }

        public void LocalClientTick(float deltaTime)
        {
            for (int i = 0; i < _localClientTickables.Count; i++)
            {
                if (_localClientTickables[i] == null)
                {
                    _localClientTickables.RemoveAt(i);
                    continue;
                }

                _localClientTickables[i].LocalClientTick(deltaTime);
            }
        }

        public void OtherClientsTick(float deltaTime)
        {
            for (int i = 0; i < _otherClientsTickables.Count; i++)
            {
                if (_otherClientsTickables[i] == null)
                {
                    _otherClientsTickables.RemoveAt(i);
                    continue;
                }

                _otherClientsTickables[i].OtherClientsTick(deltaTime);
            }
        }

        public void UnregisterClientPreTick(IClientPreTickable tickable)
        {
            _clientPreTickables.Remove(tickable);
        }

        public void UnregisterClientTick(IClientTickable tickable)
        {
            _clientTickables.Remove(tickable);
        }

        public void UnregisterClientPostTick(IClientPostTickable tickable)
        {
            _clientPostTickables.Remove(tickable);
        }

        public void UnregisterServerPreTick(IServerPreTickable tickable)
        {
            _serverPreTickables.Remove(tickable);
        }

        public void UnregisterServerTick(IServerTickable tickable)
        {
            _serverTickables.Remove(tickable);
        }

        public void UnregisterServerPostTick(IServerPostTickable tickable)
        {
            _serverPostTickables.Remove(tickable);
        }

        public void UnregisterLocalClientPreTick(ILocalClientPreTickable tickable)
        {
            _localClientPreTickables.Remove(tickable);
        }

        public void UnregisterLocalClientTick(ILocalClientTickable tickable)
        {
            _localClientTickables.Remove(tickable);
        }

        public void UnregisterLocalClientPostTick(ILocalClientPostTickable tickable)
        {
            _localClientPostTickables.Remove(tickable);
        }

        public void UnregisterOtherClientsPreTick(IOtherClientsPreTickable tickable)
        {
            _otherClientsPreTickables.Remove(tickable);
        }

        public void UnregisterOtherClientsTick(IOtherClientsTickable tickable)
        {
            _otherClientsTickables.Remove(tickable);
        }

        public void UnregisterOtherClientsPostTick(IOtherClientsPostTickable tickable)
        {
            _otherClientsPostTickables.Remove(tickable);
        }
    }
}