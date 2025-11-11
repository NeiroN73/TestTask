using Cysharp.Threading.Tasks;
using Mirror;

namespace Game.Stages
{
    public abstract class NetworkInstallerStage : NetworkBehaviour
    {
        public abstract UniTask Run();
        public virtual void Dispose() {}
    }
}