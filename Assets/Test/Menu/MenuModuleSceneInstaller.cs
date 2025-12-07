using RPGFramework.DI;
using RPGFramework.Menu;
using RPGFramework.Menu.SubMenus;
using RPGFramework.Menu.SubMenus.UI;
using UnityEngine;

namespace Test.Menu
{
    public class MenuModuleSceneInstaller : SceneInstallerBase
    {
        [SerializeField]
        private MenuUIProvider m_MenuUIProvider;

        public override void InstallBindings(IDIContainer container)
        {
            string[] sheetNames = new[]
                                  {
                                          Test.Localisation.LocalisationKeys.Generic.SHEET_NAME,
                                          Localisation.LocalisationKeys.BeginMenu.SHEET_NAME
                                  };

            IBeginMenuLocalisationArgs beginMenuLocalisationArgs = new BeginMenuLocalisationArgs(Test.Localisation.LocalisationKeys.Generic.GAME_TITLE,
                                                                                                 Localisation.LocalisationKeys.BeginMenu.NEW_GAME,
                                                                                                 Localisation.LocalisationKeys.BeginMenu.LOAD_GAME,
                                                                                                 Localisation.LocalisationKeys.BeginMenu.SETTINGS,
                                                                                                 Localisation.LocalisationKeys.BeginMenu.QUIT_GAME,
                                                                                                 sheetNames);

            container.BindSingletonFromInstance(beginMenuLocalisationArgs);

            container.BindSingletonFromInstance<IMenuUIProvider>(m_MenuUIProvider);
            container.BindTransient<IBeginMenu, BeginMenu>();
            container.BindTransient<IBeginMenuUI, BeginMenuUI>();
        }
    }
}