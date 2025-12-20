using System;
using System.Collections.Generic;
using FishNet.Object;
using Game.Creatures;
using Game.LifetimeScopes;
using GameCore.Creatures;
using GameCore.LifetimeScopes;
using GameCore.Services;
using VContainer;
using IInitializable = GameCore.Services.IInitializable;
using ITickable = GameCore.Services.ITickable;

namespace Game.Installers
{
    public static class NetworkObjectInitializeUtils
    {
        public static void InitializeServerObjects<TObject>(IEnumerable<TObject> objects,
            ServerLifetimeScope scope)
        {
            var tickService = scope.Container.Resolve<TickService>();
            
            foreach (var obj in objects)
            {
                if (obj is IServerInjectable serverInjectable)
                {
                    scope.Container.Inject(serverInjectable);
                }
                if (obj is IServerInitializable serverInitializable)
                {
                    serverInitializable.Initialize();
                }
                if (obj is IServerTickable serverTickable)
                {
                    tickService.RegisterTick(serverTickable);
                }
                if (obj is IServerDisposable serverDisposable)
                {
                    serverDisposable.Dispose();
                }
            }
        }
        
        public static void InitializeClientObjects<TObject>(IEnumerable<TObject> objects,
            ClientLifetimeScope scope, bool isController)
        {
            var tickService = scope.Container.Resolve<TickService>();
            
            foreach (var obj in objects)
            {
                if (obj is IClientInjectable clientsInjectable)
                {
                    scope.Container.Inject(clientsInjectable);
                }
                if (obj is IClientInitializable clientsInitializable)
                {
                    clientsInitializable.Initialize();
                }
                if (obj is IClientTickable clientsTickable)
                {
                    tickService.RegisterTick(clientsTickable);
                }
                if (obj is IClientDisposable clientsDisposable)
                {
                    clientsDisposable.Dispose();
                }

                if (isController)
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
        
        public static void InitializeNetworkObjects<TObject>(IReadOnlyCollection<TObject> objects, bool isController,
            ServerLifetimeScope serverScope, ClientLifetimeScope clientScope)
        {
            InitializeServerObjects(objects, serverScope);
            InitializeClientObjects(objects, clientScope, isController);
        }

        public static void InitializeObjectsFromContainer<T>(T scope, bool isController = false) where T : BaseLifetimeScope
        {
            var objects = new List<object>();
            
            objects.AddRange(scope.Container.Resolve<IEnumerable<IInjectable>>());
            objects.AddRange(scope.Container.Resolve<IEnumerable<IInitializable>>());
            objects.AddRange(scope.Container.Resolve<IEnumerable<ITickable>>());
            objects.AddRange(scope.Container.Resolve<IEnumerable<IDisposable>>());

            if (scope is ServerLifetimeScope serverLifetimeScope)
            {
                InitializeServerObjects(objects, serverLifetimeScope);
            }
            if (scope is ClientLifetimeScope clientLifetimeScope)
            {
                InitializeClientObjects(objects, clientLifetimeScope, isController);
            }
        }
    }
}