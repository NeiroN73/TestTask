using Cysharp.Threading.Tasks;
using Mirror;
using R3;

namespace Game.Stages
{
    public abstract class NetworkInstallerStage : NetworkBehaviour
    {
        protected CompositeDisposable Disposables = new CompositeDisposable();

        public virtual void Initialize() {}
        public abstract UniTask Run();
        public virtual void Dispose() {}
    }
}