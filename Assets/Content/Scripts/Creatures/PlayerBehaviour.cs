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

        protected override void Initialize()
        {
            ComponentsContainer.TryAddNetworkComponent<MoveServerComponent>().Configure(_characterController);
            ComponentsContainer.TryAddNetworkComponent<ChangeNameServerComponent>();
            
            ComponentsContainer.TryAddNetworkComponent<InputLocalClientComponent, ControllerComponent>();
            ComponentsContainer.TryAddNetworkComponent<MoveClientComponent>().Configure(_animator);
            ComponentsContainer.TryAddNetworkComponent<ChangeNameClientComponent>().Configure(_userNameText);
            
            InitializeComponents();
        }
    }
}