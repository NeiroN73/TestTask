using Cysharp.Threading.Tasks;
using FishNet.Object;
using R3;

namespace Game.Stages
{
    public abstract class NetworkInstallerStage : NetworkBehaviour
    {
        protected CompositeDisposable Disposable = new();

        public abstract UniTask Run();
    }
}