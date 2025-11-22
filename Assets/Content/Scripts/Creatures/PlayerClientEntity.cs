using Game.Components;
using Game.Configs;
using Mirror;
using UnityEngine;
using VContainer;

namespace Game.Creatures
{
    public class PlayerClientEntity : NetworkEntity, IClientsInitializable, IClientsInjectable
    {
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] public CharacterController CharacterController { get; private set; }
        
        [field: SerializeField] public ControllerComponentParent ControllerComponentParent { get; private set; }
        [field: SerializeField] public MoveComponentParent MoveComponentParent { get; private set; }
        
        
        [Inject] private PlayerConfig _playerConfig;

        public void ClientsInitialize()
        {
            MoveComponentParent.Server = new MoveServerComponent(transform, ControllerComponentParent,
                CharacterController, _playerConfig.MoveData);
            MoveComponentParent.Client = new MoveClientComponent(transform, CharacterController, Animator);

            if (ControllerComponentParent is InputComponent inputComponent)
            {
                inputComponent.LocalClient = new InputLocalClientComponent();
            }
            else if (ControllerComponentParent is AIComponent aiComponent)
            {
                aiComponent.Server = new AIServerComponent();
            }
        }
    }
}