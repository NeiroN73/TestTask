using FishNet.Object;
using Game.Creatures;
using R3;

namespace Game.Components
{
    public abstract class EntityComponent : NetworkBehaviour
    {
        protected BaseNetworkBehaviour Behaviour;
        protected CompositeDisposable Disposable = new();
        
        public void TryInitialize(BaseNetworkBehaviour behaviour)
        {
            Behaviour = behaviour;

            Initialize();
        }
        
        public void TryDispose()
        {
            Disposable.Dispose();

            Dispose();
        }
        
        protected virtual void Initialize() {}
        protected virtual void Dispose() {}
    }
}