using System;
using Cysharp.Threading.Tasks;
using GameCore.Services;
using GameCore.UI.Loading;
using Mirror;
using VContainer;

namespace Game.Services
{
    public class ConnectionService : Service
    {
        [Inject] private ScreensService _screensService;
        [Inject] private ScenesService _scenesService;

        public async UniTask HostGameAsync(Action onSuccess)
        {
            try
            {
                _screensService.OpenLoading<LoadingScreen>();
                
                await StopActiveNetworkConnection();
                await _scenesService.LoadSceneAsync(SceneConsts.Gameplay);
                
                NetworkManager.singleton.StartHost();
                
                await WaitForNetworkCondition(() => NetworkServer.active, TimeSpan.FromSeconds(20),
                    "Host activation");
                
                onSuccess?.Invoke();
            }
            catch (Exception ex)
            {
                HandleNetworkError(NetworkManager.singleton.StopHost, ex);
                throw;
            }
            finally
            {
                _screensService.CloseLoading();
            }
        }

        public async UniTask JoinGameAsync(Action onSuccess)
        {
            try
            {
                _screensService.OpenLoading<LoadingScreen>();
                
                await StopActiveNetworkConnection();
                await _scenesService.LoadSceneAsync(SceneConsts.Gameplay);
                
                NetworkManager.singleton.StartClient();
                
                await WaitForNetworkCondition(() => NetworkClient.isConnected, TimeSpan.FromSeconds(20), 
                    "Client connection");
                
                onSuccess?.Invoke();
            }
            catch (Exception ex)
            {
                HandleNetworkError(NetworkManager.singleton.StopClient, ex);
                throw;
            }
            finally
            {
                _screensService.CloseLoading();
            }
        }

        private void HandleNetworkError(Action action, Exception ex)
        {
            action?.Invoke();
            UnityEngine.Debug.LogError($"Network failed: {ex.Message}");
        }

        private async UniTask StopActiveNetworkConnection()
        {
            if (NetworkManager.singleton.isNetworkActive)
            {
                NetworkManager.singleton.StopHost();
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