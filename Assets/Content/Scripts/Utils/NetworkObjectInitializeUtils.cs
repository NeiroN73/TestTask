using System.Collections.Generic;
using FishNet.Object;
using Game.NetworkInterfaces;
using Game.Services;
using VContainer;

namespace Game.Installers
{
    public static class NetworkObjectInitializeUtils
    {
        public static void InitializeNetworkObjects<TObject>(IEnumerable<TObject> objects, 
            IObjectResolver objectResolver)
            where TObject : NetworkBehaviour
        {
            var tickService = objectResolver.Resolve<NetworkTickService>();
            
            foreach (var obj in objects)
            {
                var networkObject = obj.GetComponent<NetworkObject>();
                
                if (obj is IInjectable serverInjectable)
                {
                    objectResolver.Inject(serverInjectable);
                }
                
                if (networkObject.IsServerInitialized)
                {
                    if (obj is IServerPreInitializable serverPreInitialize)
                    {
                        serverPreInitialize.ServerPreInitialize();
                    }

                    if (obj is IServerInitializable serverInitializable)
                    {
                        serverInitializable.ServerInitialize();
                    }

                    if (obj is IServerPostInitializable serverPostInitialize)
                    {
                        serverPostInitialize.ServerPostInitialize();
                    }
                    
                    if (obj is IServerTickable serverTickable)
                    {
                        tickService.RegisterServerTick(serverTickable);
                    }
                }

                if (networkObject.IsClientInitialized)
                {
                    if (obj is IClientPreInitializable clientPreInitialize)
                    {
                        clientPreInitialize.ClientPreInitialize();
                    }

                    if (obj is IClientInitializable clientInitializable)
                    {
                        clientInitializable.ClientInitialize();
                    }

                    if (obj is IClientPostInitializable clientPostInitialize)
                    {
                        clientPostInitialize.ClientPostInitialize();
                    }
                    
                    if (obj is IClientTickable clientTickable)
                    {
                        tickService.RegisterClientTick(clientTickable);
                    }

                    if (networkObject.IsOwner)
                    {
                        if (obj is ILocalClientPreInitializable localClientPreInitialize)
                        {
                            localClientPreInitialize.LocalClientPreInitialize();
                        }

                        if (obj is ILocalClientInitializable localClientInitializable)
                        {
                            localClientInitializable.LocalClientInitialize();
                        }

                        if (obj is ILocalClientPostInitializable localClientPostInitialize)
                        {
                            localClientPostInitialize.LocalClientPostInitialize();
                        }

                        if (obj is ILocalClientTickable localClientTickable)
                        {
                            tickService.RegisterLocalClientTick(localClientTickable);
                        }
                    }
                    else
                    {
                        if (obj is IOtherClientsPreInitializable otherClientsPreInitialize)
                        {
                            otherClientsPreInitialize.OtherClientsPreInitialize();
                        }

                        if (obj is IOtherClientsInitializable otherClientsInitializable)
                        {
                            otherClientsInitializable.OtherClientsInitialize();
                        }

                        if (obj is IOtherClientsPostInitializable otherClientsPostInitialize)
                        {
                            otherClientsPostInitialize.OtherClientsPostInitialize();
                        }

                        if (obj is IOtherClientsTickable otherClientsTickable)
                        {
                            tickService.RegisterOtherClientsTick(otherClientsTickable);
                        }
                    }
                }
            }
        }

        public static void DisposeNetworkObjects<TObject>(IEnumerable<TObject> objects, 
            IObjectResolver objectResolver)
            where TObject : NetworkBehaviour
        {
            var tickService = objectResolver.Resolve<NetworkTickService>();

            foreach (var obj in objects)
            {
                var networkObject = obj.GetComponent<NetworkObject>();
                
                if (networkObject.IsServerInitialized)
                {
                    if (obj is IServerTickable serverTickable)
                    {
                        tickService.UnregisterServerTick(serverTickable);
                    }

                    if (obj is IServerDisposable serverDisposable)
                    {
                        serverDisposable.ServerDispose();
                    }
                }

                if (networkObject.IsClientInitialized)
                {
                    if (obj is IClientTickable clientTickable)
                    {
                        tickService.UnregisterClientTick(clientTickable);
                    }

                    if (obj is IClientDisposable clientDisposable)
                    {
                        clientDisposable.ClientDispose();
                    }

                    if (networkObject.IsOwner)
                    {
                        if (obj is ILocalClientTickable localClientTickable)
                        {
                            tickService.UnregisterLocalClientTick(localClientTickable);
                        }

                        if (obj is ILocalClientDisposable localClientDisposable)
                        {
                            localClientDisposable.LocalClientDispose();
                        }
                    }
                    else
                    {
                        if (obj is IOtherClientsTickable otherClientsTickable)
                        {
                            tickService.UnregisterOtherClientsTick(otherClientsTickable);
                        }

                        if (obj is IOtherClientsDisposable otherClientsDisposable)
                        {
                            otherClientsDisposable.OtherClientsDispose();
                        }
                    }
                }
            }
        }
    }
}