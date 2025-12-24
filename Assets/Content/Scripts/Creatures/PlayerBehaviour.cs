using Game.Components;
using TMPro;
using UnityEngine;

namespace Game.Creatures
{
    public class PlayerBehaviour : BaseNetworkBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private CharacterController _characterController;

        [SerializeField] private TMP_Text _userNameText; //temp test

        protected override void Configure(ComponentsContainer componentsContainer)
        {
            componentsContainer.TryAddNetworkComponent<MoveComponent>().Configure(_characterController, _animator);
            componentsContainer.TryAddNetworkComponent<InputLocalClientComponent, ControllerComponent>();
            componentsContainer.TryAddNetworkComponent<ChangeNameComponent>().Configure(_userNameText);
        }
    }
}