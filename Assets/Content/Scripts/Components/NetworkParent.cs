using FishNet.Object;
using R3;

namespace Game.Components
{
    public abstract class NetworkParent : NetworkBehaviour
    {
        protected CompositeDisposable Disposable { get; } = new();
        
        public void Dispose()
        {
            Disposable.Dispose();
        }
    }
}