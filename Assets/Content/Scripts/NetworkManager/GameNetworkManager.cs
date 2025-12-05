
using FishNet.Connection;
using FishNet.Managing;
using R3;

namespace Game.NetworkManagers
{
    public class GameNetworkManager// : NetworkManager
    {
        private readonly Subject<NetworkConnection> _clientConnected = new();
        private readonly Subject<NetworkConnection> _clientDisconnected = new();
        public Observable<NetworkConnection> ClientConnected => _clientConnected;
        public Observable<NetworkConnection> ClientDisconnected => _clientDisconnected;
        
        // public override void OnServerConnect(NetworkConnection conn)
        // {
        //     _clientConnected.OnNext(conn);
        // }
        //
        // public override void OnServerDisconnect(NetworkConnection conn)
        // {
        //     _clientDisconnected.OnNext(conn);
        // }
    }
}