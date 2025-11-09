using Game.NetworkManagers;
using Game.Services;
using Game.Stages;
using GameCore.LifetimeScopes;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game.LifetimeScopes
{
    //через parent инъекция не работает, так что через наследование сделал, ощущение, что раньше работало хммм
    public class AppLifetimeScope : CoreLifetimeScope
    {
        [SerializeField] private AssetLabelReference _gameConfigsAssetLabel;
        [SerializeField] private GameNetworkManager _gameNetworkManager;
        
        protected override void RegisterConfigs()
        {
            RegisterConfigs(_gameConfigsAssetLabel);
        }

        protected override void RegisterStages()
        {
            base.RegisterStages();
            
            Register<SpawnPlayerStage>();//todo stages components
            Register<RunNetworkStage>();
        }
        
        protected override void RegisterServices()
        {
            base.RegisterServices();
            
            Register<NetworkService>();
            Register(_gameNetworkManager);
        }
    }
}