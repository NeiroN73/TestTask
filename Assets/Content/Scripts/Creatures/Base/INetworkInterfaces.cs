using System;
using GameCore.Services;

namespace Game.Creatures
{
    public interface IClientInjectable { }
    public interface IServerInjectable { }
    public interface IClientPreInitializable : IPreInitializable { }
    public interface IClientInitializable : IInitializable { }
    public interface IClientPostInitializable : IPostInitializable { }
    public interface IClientTickable : ITickable { }
    public interface IClientDisposable : IDisposable { }
    public interface IServerPreInitializable : IPreInitializable { }
    public interface IServerInitializable : IInitializable { }
    public interface IServerPostInitializable : IPostInitializable { }
    public interface IServerTickable : ITickable { }
    public interface IServerDisposable : IDisposable { }
    public interface ILocalClientPreInitializable : IPreInitializable { }
    public interface ILocalClientInitializable : IInitializable { }
    public interface ILocalClientPostInitializable : IPostInitializable { }
    public interface ILocalClientTickable : ITickable { }
    public interface ILocalClientDisposable : IDisposable { }
    public interface IOtherClientsPreInitializable : IPreInitializable { }
    public interface IOtherClientsInitializable : IInitializable { }
    public interface IOtherClientsPostInitializable : IPostInitializable { }
    public interface IOtherClientsTickable : ITickable {}
    public interface IOtherClientsDisposable : IDisposable {}
}