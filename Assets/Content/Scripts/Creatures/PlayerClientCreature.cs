using Game.Components;
using Game.Configs;
using UnityEngine;
using VContainer;

namespace Game.Creatures
{
    public class PlayerClientCreature : ClientNetworkCreature
    {
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] public CharacterController CharacterController { get; private set; }
        
        [Inject] private PlayerConfig _playerConfig;

        protected override void Initialize()
        {
            GetCreatureComponentByType<MoveComponent>().Init(CharacterController, _playerConfig.MoveData, Animator);
        }
    }
}