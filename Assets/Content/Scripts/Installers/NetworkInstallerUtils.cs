using System;
using System.Collections.Generic;
using FishNet.Object;
using Game.Creatures;
using GameCore.Creatures;
using GameCore.LifetimeScopes;
using GameCore.Services;
using VContainer;

namespace Game.Installers
{
    public static class NetworkInstallerUtils
    {
        public static void ProcessInterfaces<TObject>(this NetworkBehaviour networkBehaviour, IEnumerable<TObject> objects,
            BaseLifetimeScope scope)
        {
            var tickService = scope.Container.Resolve<TickService>();
            
            foreach (var obj in objects)
            {
                if (obj is IServerInjectable serverInjectable)
                {
                    scope.Container.Inject(serverInjectable);
                }
                if (obj is IClientsInjectable clientsInjectable)
                {
                    scope.Container.Inject(clientsInjectable);
                }
                
                if (obj is IServerInitializable serverInitializable)
                {
                    serverInitializable.Initialize();
                }
                if (obj is IClientsInitializable clientsInitializable)
                {
                    clientsInitializable.Initialize();
                }
                
                if (obj is IServerTickable serverTickable)
                {
                    tickService.RegisterTick(serverTickable);
                }
                if (obj is IClientTickable clientsTickable)
                {
                    tickService.RegisterTick(clientsTickable);
                }

                if (obj is IServerDisposable serverDisposable)
                {
                    serverDisposable.Dispose();
                }
                if (obj is IClientsDisposable clientsDisposable)
                {
                    clientsDisposable.Dispose();
                }

                if (networkBehaviour.IsOwner)
                {
                    if (obj is ILocalClientInitializable localClientInitializable)
                    {
                        localClientInitializable.Initialize();
                    }
                    if (obj is ILocalClientTickable localClientTickable)
                    {
                        tickService.RegisterTick(localClientTickable);
                    }
                    if (obj is ILocalClientDisposable localClientDisposable)
                    {
                        localClientDisposable.Dispose();
                    }
                }
                else
                {
                    if (obj is IOtherClientsInitializable otherClientsInitializable)
                    {
                        otherClientsInitializable.Initialize();
                    }
                    if (obj is IOtherClientsTickable otherClientsTickable)
                    {
                        tickService.RegisterTick(otherClientsTickable);
                    }
                    if (obj is IOtherClientsDisposable otherClientsDisposable)
                    {
                        otherClientsDisposable.Dispose();
                    }
                }
            }
        }

        public static void ProcessInterfacesFromContainer(this NetworkBehaviour networkBehaviour, BaseLifetimeScope scope)
        {
            var injectables = scope.Container.Resolve<IEnumerable<IInjectable>>();
            ProcessInterfaces(networkBehaviour, injectables, scope);
            var initializables = scope.Container.Resolve<IEnumerable<IInitializable>>();
            ProcessInterfaces(networkBehaviour, initializables, scope);
            var tickables = scope.Container.Resolve<IEnumerable<ITickable>>();
            ProcessInterfaces(networkBehaviour, tickables, scope);
            var disposables = scope.Container.Resolve<IEnumerable<IDisposable>>();
            ProcessInterfaces(networkBehaviour, disposables, scope);
        }
    }
}