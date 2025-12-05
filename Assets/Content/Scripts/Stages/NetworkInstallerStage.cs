using Cysharp.Threading.Tasks;
using FishNet.Object;
using R3;

namespace Game.Stages
{
    public abstract class NetworkInstallerStage : NetworkBehaviour
    {
        protected CompositeDisposable Disposables = new CompositeDisposable();

        public virtual void ServerInitialize() {}
        public abstract UniTask ServerRun();
    }
}