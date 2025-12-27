using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.Stages
{
    public abstract class InstallerStage : MonoBehaviour
    {
        public abstract UniTask Run();
    }
}