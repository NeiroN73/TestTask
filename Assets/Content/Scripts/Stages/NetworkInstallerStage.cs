using Cysharp.Threading.Tasks;
using R3;

namespace Game.Stages
{
    public abstract class NetworkInstallerStage
    {
        protected CompositeDisposable Disposable = new();

        public abstract UniTask Run();
    }
}