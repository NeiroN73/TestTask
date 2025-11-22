namespace Game.Creatures
{
    // public interface IClientsTag
    // {
    // }
    //
    // public interface IServerTag
    // {
    // }
    //
    // public interface ILocalClientTag
    // {
    // }
    //
    // public interface IOtherClientsTag
    // {
    // }
    
    public interface IClientsInjectable
    {
    }

    public interface IServerInjectable
    {
    }
    
    public interface IClientsInitializable
    {
        void ClientsInitialize();
    }

    public interface IClientTickable
    {
        void ClientTick(float deltaTime);
    }

    public interface IClientsDisposable
    {
        void ClientsDispose();
    }
    
    public interface IServerInitializable
    {
        void ServerInitialize();
    }

    public interface IServerTickable
    {
        void ServerTick(float deltaTime);
    }

    public interface IServerDisposable
    {
        void ServerDispose();
    }
    
    public interface ILocalClientInitializable
    {
        void LocalClientInitialize();
    }

    public interface ILocalClientTickable
    {
        void LocalClientTick(float deltaTime);
    }

    public interface ILocalClientDisposable
    {
        void LocalClientDispose();
    }
    
    public interface IOtherClientsInitializable
    {
        void OtherClientsInitialize();
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