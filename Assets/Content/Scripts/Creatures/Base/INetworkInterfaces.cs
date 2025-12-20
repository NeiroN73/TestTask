using System;
using GameCore.Services;

namespace Game.Creatures
{
    public interface IClientInjectable
    {
    }

    public interface IServerInjectable
    {
    }
    
    public interface IClientInitializable : IInitializable
    {
    }

    public interface IClientTickable : ITickable
    {
    }

    public interface IClientDisposable : IDisposable
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