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
        
        [Inject] private PlayerConfig _playerConfig;

        public void ClientsInitialize()
        {
            GetEntityComponentByType<MoveComponent>().Initialize(CharacterController, _playerConfig.MoveData, Animator);
        }
    }

    public class PlayerServerEntity : NetworkEntity
    {
        
    }

    public class PlayerEntity : NetworkEntity
    {
        
    }

    [RequireComponent(typeof(NetworkIdentity))]
    public abstract class NetworkParent<T> : NetworkBehaviour where T : NetworkBehaviour
    {
        
    }
    
    public abstract class NetworkCreatureParent : NetworkBehaviour
    {
    }

    [RequireComponent(typeof(PlayerClientEntity))]
    [RequireComponent(typeof(PlayerServerEntity))]
    public class PlayerCreatureParent : NetworkCreatureParent, IHaveClientCreature<PlayerClientEntity>,
        IHaveServerCreature<PlayerServerEntity>
    {
        public PlayerClientEntity Client { get; set; }
        public PlayerServerEntity Server { get; set; }
    }
    
    public interface IHaveClientCreature<T> where T : NetworkEntity
    {
        public T Client { get; set; }
    }
    
    public interface IHaveServerCreature<T> where T : NetworkEntity
    {
        public T Server { get; set; }
    }
}