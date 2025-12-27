using Cysharp.Threading.Tasks;
using FishNet.Object;

namespace Game.Stages
{
    public abstract class NetworkInstallerStage : NetworkBehaviour
    {
        public abstract UniTask Run();
    }
}