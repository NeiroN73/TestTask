using System.Collections.Generic;
using Game.Stages;
using GameCore.LifetimeScopes;
using UnityEngine;

namespace Game.LifetimeScopes
{
    public class GameplayLifetimeScope : BaseLifetimeScope
    {
        [SerializeField] private List<InstallerStage> _installerStages = new();
        
        protected override void RegisterStages()
        {
            foreach (var stage in _installerStages)
            {
                Register(stage);
            }
        }
    }
}