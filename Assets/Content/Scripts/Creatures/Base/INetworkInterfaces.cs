using System;
using GameCore.Services;

namespace Game.Creatures
{
    public interface IClientsInjectable
    {
    }

    public interface IServerInjectable
    {
    }
    
    public interface IClientsInitializable : IInitializable
    {
    }

    public interface IClientTickable : ITickable
    {
    }

    public interface IClientsDisposable : IDisposable
    {
    }
    
    public interface IServerInitializable : IInitializable
    {
    }

    public interface IServerTickable : ITickable
    {
    }

    public interface IServerDisposable : IDisposable
    {
    }
    
    public interface ILocalClientInitializable : IInitializable
    {
    }

    public interface ILocalClientTickable : ITickable
    {
    }

    public interface ILocalClientDisposable : IDisposable
    {
    }
    
    public interface IOtherClientsInitializable : IInitializable
    {
    }

    public interface IOtherClientsTickable : ITickable
    {
    }

    public interface IOtherClientsDisposable : IDisposable
    {
    }
}