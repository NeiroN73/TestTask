using Game.Components;
using Game.Configs;
using TMPro;
using UnityEngine;
using VContainer;

namespace Game.Creatures
{
    public class PlayerBehaviour : BaseNetworkBehaviour, IClientInjectable
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private CharacterController _characterController;

        [SerializeField] private TMP_Text _userNameText; //temp test
        
        [Inject] private PlayerConfig _playerConfig;
        
        protected override void ServerInitialize()
        {
            var moveServerElement = new MoveServerElement(transform, _characterController, _playerConfig.MoveData);
            var changeNameServerElement = new ChangeNameServerElement();
            
            ServerInitializeElements(moveServerElement, changeNameServerElement);
        }

        protected override void ClientInitialize()
        {
            var moveClientElement = new MoveClientElement(transform, _animator);
            var changeNameClientElement = new ChangeNameClientElement(_userNameText);
            ClientInitializeElements(moveClientElement, changeNameClientElement);
            
            if (IsOwner)
            {
                var controllerElement = new InputLocalClientElement(LocalConnection);
                ClientInitializeElements(controllerElement);
            }
        }
    }
}