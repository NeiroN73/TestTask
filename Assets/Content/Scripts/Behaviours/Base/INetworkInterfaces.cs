namespace Game.NetworkInterfaces
{
    public interface IInjectable { }
    
    public interface IClientPreInitializable
    {
        void ClientPreInitialize();
    }
    
    public interface IClientInitializable
    {
        void ClientInitialize();
    }
    
    public interface IClientPostInitializable
    {
        void ClientPostInitialize();
    }
    
    public interface IClientTickable
    {
        void ClientTick(float deltaTime);
    }
    
    public interface IClientDisposable
    {
        void ClientDispose();
    }
    
    public interface IServerPreInitializable
    {
        void ServerPreInitialize();
    }
    
    public interface IServerInitializable
    {
        void ServerInitialize();
    }
    
    public interface IServerPostInitializable
    {
        void ServerPostInitialize();
    }
    
    public interface IServerTickable
    {
        void ServerTick(float deltaTime);
    }
    
    public interface IServerDisposable
    {
        void ServerDispose();
    }
    
    public interface ILocalClientPreInitializable
    {
        void LocalClientPreInitialize();
    }
    
    public interface ILocalClientInitializable
    {
        void LocalClientInitialize();
    }
    
    public interface ILocalClientPostInitializable
    {
        void LocalClientPostInitialize();
    }
    
    public interface ILocalClientTickable
    {
        void LocalClientTick(float deltaTime);
    }
    
    public interface ILocalClientDisposable
    {
        void LocalClientDispose();
    }
    
    public interface IOtherClientsPreInitializable
    {
        void OtherClientsPreInitialize();
    }
    
    public interface IOtherClientsInitializable
    {
        void OtherClientsInitialize();
    }
    
    public interface IOtherClientsPostInitializable
    {
        void OtherClientsPostInitialize();
    }
    
    public interface IOtherClientsTickable
    {
        void OtherClientsTick(float deltaTime);
    }
    
    public interface IOtherClientsDisposable
    {
        void OtherClientsDispose();
    }
}