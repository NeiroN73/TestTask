using Game.Services;
using GameCore.LifetimeScopes;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game.LifetimeScopes
{
    public class AppLifetimeScope : CoreLifetimeScope
    {
        [SerializeField] private AssetLabelReference _gameConfigsAssetLabel;
        
        protected override void RegisterConfigs()
        {
            base.RegisterConfigs();
            
            RegisterConfigs(_gameConfigsAssetLabel);
        }
        
        protected override void RegisterServices()
        {
            base.RegisterServices();
            
            Register<ConnectionService>();
        }
    }
}