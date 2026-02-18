using System.Collections.Generic;
using Game.NetworkInterfaces;

namespace Game.Services
{
    public class NetworkTickService : NetworkService
    {
        private readonly List<IClientTickable> _clientTickables = new();
        private readonly List<IServerTickable> _serverTickables = new();
        private readonly List<ILocalClientTickable> _localClientTickables = new();
        private readonly List<IOtherClientsTickable> _otherClientsTickables = new();

        public void RegisterClientTick(IClientTickable tickable) => 
            _clientTickables.Add(tickable);

        public void RegisterServerTick(IServerTickable tickable) => 
            _serverTickables.Add(tickable);

        public void RegisterLocalClientTick(ILocalClientTickable tickable) => 
            _localClientTickables.Add(tickable);

        public void RegisterOtherClientsTick(IOtherClientsTickable tickable) => 
            _otherClientsTickables.Add(tickable);

        public void UnregisterClientTick(IClientTickable tickable) => 
            _clientTickables.Remove(tickable);

        public void UnregisterServerTick(IServerTickable tickable) => 
            _serverTickables.Remove(tickable);

        public void UnregisterLocalClientTick(ILocalClientTickable tickable) => 
            _localClientTickables.Remove(tickable);

        public void UnregisterOtherClientsTick(IOtherClientsTickable tickable) => 
            _otherClientsTickables.Remove(tickable);

        public void ClientTick(float deltaTime) => 
            TickCollection(_clientTickables, t => t.ClientTick(deltaTime));

        public void ServerTick(float deltaTime) => 
            TickCollection(_serverTickables, t => t.ServerTick(deltaTime));

        public void LocalClientTick(float deltaTime) => 
            TickCollection(_localClientTickables, t => t.LocalClientTick(deltaTime));

        public void OtherClientsTick(float deltaTime) => 
            TickCollection(_otherClientsTickables, t => t.OtherClientsTick(deltaTime));

        private void TickCollection<T>(List<T> tickables, System.Action<T> tickAction) where T : class
        {
            for (int i = 0; i < tickables.Count; i++)
            {
                if (tickables[i] == null)
                {
                    tickables.RemoveAt(i);
                    continue;
                }

                tickAction(tickables[i]);
            }
        }
    }
}