using System;
using Cysharp.Threading.Tasks;
using FishNet;
using FishNet.Managing;
using GameCore.Services;
using GameCore.UI.Loading;
using VContainer;
using UnityEngine;

namespace Game.Services
{
    public class ConnectionService : Service
    {
        [Inject] private ScreensService _screensService;
        [Inject] private ScenesService _scenesService;

        private NetworkManager _networkManager;

        private NetworkManager NetworkManager
        {
            get
            {
                if (_networkManager == null)
                    _networkManager = InstanceFinder.NetworkManager;
                return _networkManager;
            }
        }

        public async UniTask HostGameAsync(Action onSuccess)
        {
            try
            {
                _screensService.OpenLoading<LoadingScreen>();

                await StopActiveNetworkConnection();
                await _scenesService.LoadSceneAsync(SceneConsts.Gameplay);

                NetworkManager.ServerManager.StartConnection();
                NetworkManager.ClientManager.StartConnection();

                await WaitForNetworkCondition(() => NetworkManager.ServerManager.Started, TimeSpan.FromSeconds(20),
                    "Host activation");

                onSuccess?.Invoke();
            }
            catch (Exception ex)
            {
                HandleNetworkError(() =>
                {
                    NetworkManager.ServerManager.StopConnection(true);
                    NetworkManager.ClientManager.StopConnection();
                }, ex);
                throw;
            }
            finally
            {
                _screensService.CloseLoading();
            }
        }

        public async UniTask JoinGameAsync(string address, ushort port, Action onSuccess)
        {
            try
            {
                _screensService.OpenLoading<LoadingScreen>();

                await StopActiveNetworkConnection();
                await _scenesService.LoadSceneAsync(SceneConsts.Gameplay);

                NetworkManager.ClientManager.StartConnection(address, port);

                await WaitForNetworkCondition(() => NetworkManager.ClientManager.Started, TimeSpan.FromSeconds(20),
                    "Client connection");

                onSuccess?.Invoke();
            }
            catch (Exception ex)
            {
                HandleNetworkError(() => NetworkManager.ClientManager.StopConnection(), ex);
                throw;
            }
            finally
            {
                _screensService.CloseLoading();
            }
        }

        public async UniTask JoinGameAsync(Action onSuccess)
        {
            string address = NetworkManager.TransportManager.Transport.GetClientAddress();
            ushort port = NetworkManager.TransportManager.Transport.GetPort();

            await JoinGameAsync(address, port, onSuccess);
        }

        private void HandleNetworkError(Action action, Exception ex)
        {
            action?.Invoke();
            Debug.LogError($"Network failed: {ex.Message}");
        }

        private async UniTask StopActiveNetworkConnection()
        {
            if (NetworkManager.ServerManager.Started || NetworkManager.ClientManager.Started)
            {
                NetworkManager.ServerManager.StopConnection(true);
                NetworkManager.ClientManager.StopConnection();

                await UniTask.Delay(1000);
            }
        }

        private async UniTask WaitForNetworkCondition(Func<bool> condition, TimeSpan timeout, string operationName)
        {
            try
            {
                await UniTask.WaitUntil(condition).Timeout(timeout);
            }
            catch (TimeoutException)
            {
                throw new TimeoutException($"{operationName} timeout after {timeout.TotalSeconds} seconds");
            }
        }
    }
}