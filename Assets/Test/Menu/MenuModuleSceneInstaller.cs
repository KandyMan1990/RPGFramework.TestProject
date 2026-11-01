using RPGFramework.DI;
using RPGFramework.Menu;
using UnityEngine;

namespace Test.Menu
{
    public class MenuModuleSceneInstaller : SceneInstallerBase
    {
        [SerializeField]
        private MenuUIProvider m_MenuUIProvider;

        [SerializeField]
        private MenuTransitionConfig m_TransitionConfig;

        public override void InstallBindings(IDIContainer container)
        {
            container.BindSingletonFromInstance<IMenuUIProvider>(m_MenuUIProvider);
            container.BindSingletonFromInstance<IMenuTransitionConfig>(m_TransitionConfig);
        }
    }
}
