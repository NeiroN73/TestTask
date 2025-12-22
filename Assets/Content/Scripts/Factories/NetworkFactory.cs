using Content.Scripts.EventBus;
using FishNet.Object;

namespace Content.Scripts.Factories
{
    public class NetworkFactory : NetworkBehaviour
    {
        protected NetworkEventDisposable Disposable = new();
    }
}