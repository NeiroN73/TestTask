using System.Collections.Generic;
using FishNet.Object;
using Game.Creatures;
using Game.Services;
using VContainer;

namespace Game.Installers
{
    public static class NetworkObjectInitializeUtils
    {
        public static void InitializeNetworkObjects<TObject>(IEnumerable<TObject> objects, IObjectResolver objectResolver)
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

                    if (obj is IServerPreTickable serverPreTickable)
                    {
                        tickService.RegisterServerPreTick(serverPreTickable);
                    }

                    if (obj is IServerTickable serverTickable)
                    {
                        tickService.RegisterServerTick(serverTickable);
                    }

                    if (obj is IServerPostTickable serverPostTickable)
                    {
                        tickService.RegisterServerPostTick(serverPostTickable);
                    }

                    if (obj is IServerDisposable serverDisposable)
                    {
                        serverDisposable.ServerDispose();
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

                    if (obj is IClientPreTickable clientPreTickable)
                    {
                        tickService.RegisterClientPreTick(clientPreTickable);
                    }

                    if (obj is IClientTickable clientTickable)
                    {
                        tickService.RegisterClientTick(clientTickable);
                    }

                    if (obj is IClientPostTickable clientPostTickable)
                    {
                        tickService.RegisterClientPostTick(clientPostTickable);
                    }

                    if (obj is IClientDisposable clientDisposable)
                    {
                        clientDisposable.ClientDispose();
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

                        if (obj is ILocalClientPreTickable localClientPreTickable)
                        {
                            tickService.RegisterLocalClientPreTick(localClientPreTickable);
                        }

                        if (obj is ILocalClientTickable localClientTickable)
                        {
                            tickService.RegisterLocalClientTick(localClientTickable);
                        }

                        if (obj is ILocalClientPostTickable localClientPostTickable)
                        {
                            tickService.RegisterLocalClientPostTick(localClientPostTickable);
                        }

                        if (obj is ILocalClientDisposable localClientDisposable)
                        {
                            localClientDisposable.LocalClientDispose();
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

                        if (obj is IOtherClientsPreTickable otherClientsPreTickable)
                        {
                            tickService.RegisterOtherClientsPreTick(otherClientsPreTickable);
                        }

                        if (obj is IOtherClientsTickable otherClientsTickable)
                        {
                            tickService.RegisterOtherClientsTick(otherClientsTickable);
                        }

                        if (obj is IOtherClientsPostTickable otherClientsPostTickable)
                        {
                            tickService.RegisterOtherClientsPostTick(otherClientsPostTickable);
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