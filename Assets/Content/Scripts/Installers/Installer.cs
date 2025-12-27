using GameCore.LifetimeScopes;
using UnityEngine;

namespace Game.Installers
{
    public abstract class Installer : MonoBehaviour
    {
        [SerializeField] protected BaseLifetimeScope LifetimeScope;

        protected void Awake()
        {
            Initialize();
        }

        protected virtual void Initialize() {}
    }
}