using VContainer;

namespace Game.Creatures
{
    public interface INetworkInterface { }

    public interface IInjectable { }
    
    public interface IClientPreInitializable : INetworkInterface
    {
        void ClientPreInitialize();
    }
    
    public interface IClientInitializable : INetworkInterface
    {
        void ClientInitialize();
    }
    
    public interface IClientPostInitializable : INetworkInterface
    {
        void ClientPostInitialize();
    }
    
    public interface IClientPreTickable : INetworkInterface
    {
        void ClientPreTick(float deltaTime);
    }
    
    public interface IClientTickable : INetworkInterface
    {
        void ClientTick(float deltaTime);
    }
    
    public interface IClientPostTickable : INetworkInterface
    {
        void ClientPostTick(float deltaTime);
    }
    
    public interface IClientDisposable : INetworkInterface
    {
        void ClientDispose();
    }
    
    public interface IServerPreInitializable : INetworkInterface
    {
        void ServerPreInitialize();
    }
    
    public interface IServerInitializable : INetworkInterface
    {
        void ServerInitialize();
    }
    
    public interface IServerPostInitializable : INetworkInterface
    {
        void ServerPostInitialize();
    }
    
    public interface IServerPreTickable : INetworkInterface
    {
        void ServerPreTick(float deltaTime);
    }
    
    public interface IServerTickable : INetworkInterface
    {
        void ServerTick(float deltaTime);
    }
    
    public interface IServerPostTickable : INetworkInterface
    {
        void ServerPostTick(float deltaTime);
    }
    
    public interface IServerDisposable : INetworkInterface
    {
        void ServerDispose();
    }
    
    public interface ILocalClientPreInitializable : INetworkInterface
    {
        void LocalClientPreInitialize();
    }
    
    public interface ILocalClientInitializable : INetworkInterface
    {
        void LocalClientInitialize();
    }
    
    public interface ILocalClientPostInitializable : INetworkInterface
    {
        void LocalClientPostInitialize();
    }
    
    public interface ILocalClientPreTickable : INetworkInterface
    {
        void LocalClientPreTick(float deltaTime);
    }
    
    public interface ILocalClientTickable : INetworkInterface
    {
        void LocalClientTick(float deltaTime);
    }
    
    public interface ILocalClientPostTickable : INetworkInterface
    {
        void LocalClientPostTick(float deltaTime);
    }
    
    public interface ILocalClientDisposable : INetworkInterface
    {
        void LocalClientDispose();
    }
    
    public interface IOtherClientsPreInitializable : INetworkInterface
    {
        void OtherClientsPreInitialize();
    }
    
    public interface IOtherClientsInitializable : INetworkInterface
    {
        void OtherClientsInitialize();
    }
    
    public interface IOtherClientsPostInitializable : INetworkInterface
    {
        void OtherClientsPostInitialize();
    }
    
    public interface IOtherClientsPreTickable : INetworkInterface
    {
        void OtherClientsPreTick(float deltaTime);
    }
    
    public interface IOtherClientsTickable : INetworkInterface
    {
        void OtherClientsTick(float deltaTime);
    }
    
    public interface IOtherClientsPostTickable : INetworkInterface
    {
        void OtherClientsPostTick(float deltaTime);
    }
    
    public interface IOtherClientsDisposable : INetworkInterface
    {
        void OtherClientsDispose();
    }
}