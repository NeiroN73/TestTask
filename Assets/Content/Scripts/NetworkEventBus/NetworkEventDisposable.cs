using System.Collections.Generic;

namespace Game.NetworkEventBus
{
    public class NetworkEventDisposable
    {
        private readonly List<NetworkEventHandler> _handlers = new();

        public void Add(NetworkEventHandler handler)
        {
            _handlers.Add(handler);
        }
        
        public void Dispose()
        {
            foreach (var handler in _handlers)
            {
                handler.NetworkEventBus.Unsubscribe(handler.Action, handler.Type);
            }
        }
    }
}