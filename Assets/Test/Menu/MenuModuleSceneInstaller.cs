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
            BindLocalisationArgs(container);

            container.BindSingletonFromInstance<IMenuUIProvider>(m_MenuUIProvider);
            container.BindTransient<IBeginMenu, BeginMenu>();
            container.BindTransient<IBeginMenuUI, BeginMenuUI>();
            container.BindTransient<IConfigMenu, ConfigMenu>();
            container.BindTransient<IConfigMenuUI, ConfigMenuUI>();
        }

        private static void BindLocalisationArgs(IDIContainer container)
        {
            string[] beginSheetNames = new[]
                                       {
                                               Test.Localisation.LocalisationKeys.Generic.SHEET_NAME,
                                               Localisation.LocalisationKeys.BeginMenu.SHEET_NAME
                                       };

            IBeginMenuLocalisationArgs beginMenuLocalisationArgs = new BeginMenuLocalisationArgs(Test.Localisation.LocalisationKeys.Generic.GAME_TITLE,
                                                                                                 Localisation.LocalisationKeys.BeginMenu.NEW_GAME,
                                                                                                 Localisation.LocalisationKeys.BeginMenu.LOAD_GAME,
                                                                                                 Test.Localisation.LocalisationKeys.Generic.SETTINGS,
                                                                                                 Localisation.LocalisationKeys.BeginMenu.QUIT_GAME,
                                                                                                 beginSheetNames);

            string[] configSheetNames = new[]
                                        {
                                                Test.Localisation.LocalisationKeys.Generic.SHEET_NAME,
                                                Localisation.LocalisationKeys.ConfigMenu.SHEET_NAME
                                        };

            IConfigMenuLocalisationArgs configMenuLocalisationArgs = new ConfigMenuLocalisationArgs(Test.Localisation.LocalisationKeys.Generic.SETTINGS,
                                                                                                    Localisation.LocalisationKeys.ConfigMenu.LANGUAGE_TITLE,
                                                                                                    Localisation.LocalisationKeys.ConfigMenu.LANGUAGE,
                                                                                                    Localisation.LocalisationKeys.ConfigMenu.CONTROLS,
                                                                                                    Localisation.LocalisationKeys.ConfigMenu.MUSIC_VOLUME,
                                                                                                    Localisation.LocalisationKeys.ConfigMenu.SFX_VOLUME,
                                                                                                    Localisation.LocalisationKeys.ConfigMenu.BATTLE_MESSAGE_SPEED,
                                                                                                    Localisation.LocalisationKeys.ConfigMenu.FIELD_MESSAGE_SPEED,
                                                                                                    configSheetNames);

            container.BindSingletonFromInstance(beginMenuLocalisationArgs);
            container.BindSingletonFromInstance(configMenuLocalisationArgs);
        }
    }
}