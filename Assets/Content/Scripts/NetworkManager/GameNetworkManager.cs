using Mirror;
using R3;

namespace Game.NetworkManagers
{
    public class GameNetworkManager : NetworkManager
    {
        private readonly Subject<NetworkConnectionToClient> _clientConnected = new();
        private readonly Subject<NetworkConnectionToClient> _clientDisconnected = new();
        public Observable<NetworkConnectionToClient> ClientConnected => _clientConnected;
        public Observable<NetworkConnectionToClient> ClientDisconnected => _clientDisconnected;
        
        public override void OnServerConnect(NetworkConnectionToClient conn)
        {
            _clientConnected.OnNext(conn);
        }

        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            _clientDisconnected.OnNext(conn);
        }
    }
}