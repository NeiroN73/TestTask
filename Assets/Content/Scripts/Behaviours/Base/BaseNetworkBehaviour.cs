using FishNet.Object;
using Game.Installers;
using Game.LifetimeScopes;
using TriInspector;
using VContainer;
using VContainer.Unity;

namespace Game.Behaviours
{
    public abstract class BaseNetworkBehaviour : NetworkBehaviour
    {
        private IObjectResolver _objectResolver;
        private ComponentsContainer _componentsContainer;

        public void Initialize(IObjectResolver objectResolver)
        {
            _objectResolver = objectResolver;
            
            _componentsContainer = new(gameObject);
            Configure(_componentsContainer);
            InitializeComponents();
        }

        //приходится отсюда инитить, потому что иначе SyncVar не сможет обновлять переменные игроков при подключении,
        //todo: надо переделать, так как це костыль :(
        public override void OnStartClient()
        {
            base.OnStartClient();

            _objectResolver = LifetimeScope.Find<GameplayLifetimeScope>().Container;
            _componentsContainer = new(gameObject);
            Configure(_componentsContainer);
            InitializeComponents();
        }

        protected virtual void Configure(ComponentsContainer componentsContainer)
        {
        }
        
        private void InitializeComponents()
        {
            var components = _componentsContainer.Components;
            foreach (var component in components)
            {
                component.Configure(_componentsContainer);
            }
            
            NetworkObjectInitializeUtils.InitializeNetworkObjects(components, _objectResolver);
        }

        //в fishnet нельзя добавлять компоненты в рантайме, во прикол
        //todo: автоматизировать, чтобы объекты сами обновлялись
#if UNITY_EDITOR
        [Button]
        private void UpdateNetworkComponents()
        {
            var componentsContainer = new ComponentsContainer(gameObject);
            Configure(componentsContainer);
        }
#endif
    }
}