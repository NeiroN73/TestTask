using Cysharp.Threading.Tasks;
using Mirror;

namespace Game.Stages
{
    public abstract class InstallerStage : NetworkBehaviour
    {
        public abstract UniTask Initialize();
    }
}