using System.Collections.Generic;
using System.Linq;
using Game.Stages;
using GameCore.LifetimeScopes;
using UnityEngine;

namespace Game.Installers
{
    public abstract class Installer : MonoBehaviour
    {
        [SerializeField] protected BaseLifetimeScope LifetimeScope;
        protected List<InstallerStage> Stages = new();

        protected void Awake()
        {
            GatherStages();
            RunStages();
            
            Initialize();
        }

        protected virtual void Initialize() {}

        private void GatherStages()
        {
            Stages = GetComponentsInChildren<InstallerStage>().ToList();
        }

        private async void RunStages()
        {
            foreach (var stage in Stages)
            {
                await stage.Run();
            }
        }

        private void OnDestroy()
        {
            foreach (var stage in Stages)
            {
                stage.Dispose();
            }
        }
    }
}