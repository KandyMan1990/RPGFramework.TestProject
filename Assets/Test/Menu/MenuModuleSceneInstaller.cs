using RPGFramework.DI;
using RPGFramework.Menu;
using RPGFramework.Menu.SharedTypes;
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
            BindLocalisationArgs(container);

            container.BindSingletonFromInstance<IMenuUIProvider>(m_MenuUIProvider);
            container.BindSingletonFromInstance<IMenuTransitionConfig>(m_TransitionConfig);
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

            string[] languageSheetNames = new[]
                                          {
                                              Test.Localisation.LocalisationKeys.Generic.SHEET_NAME,
                                              Localisation.LocalisationKeys.ConfigMenu.SHEET_NAME
                                          };

            ILanguageMenuLocalisationArgs languageMenuLocalisationArgs = new LanguageMenuLocalisationArgs(Localisation.LocalisationKeys.ConfigMenu.LANGUAGE_TITLE,
                                                                                                          Localisation.LocalisationKeys.ConfigMenu.LANGUAGE,
                                                                                                          languageSheetNames);

            string[] partySheetNames = new[]
                                       {
                                           Test.Localisation.LocalisationKeys.Generic.SHEET_NAME,
                                           Localisation.LocalisationKeys.PartyMenu.SHEET_NAME,
                                           Localisation.LocalisationKeys.Locations.SHEET_NAME
                                       };

            IPartyMenuLocalisationArgs partyMenuLocalisationArgs = new PartyMenuLocalisationArgs(Test.Localisation.LocalisationKeys.Generic.SETTINGS,
                                                                                                 Localisation.LocalisationKeys.PartyMenu.SAVE,
                                                                                                 Localisation.LocalisationKeys.PartyMenu.LOAD,
                                                                                                 Localisation.LocalisationKeys.PartyMenu.TIME,
                                                                                                 partySheetNames);

            string[] saveSheetNames = new[]
                                      {
                                          Test.Localisation.LocalisationKeys.Generic.SHEET_NAME,
                                          Localisation.LocalisationKeys.SaveMenu.SHEET_NAME,
                                          Localisation.LocalisationKeys.Locations.SHEET_NAME
                                      };

            ISaveMenuLocalisationArgs saveMenuLocalisationArgs = new SaveMenuLocalisationArgs(Localisation.LocalisationKeys.SaveMenu.SAVE_TITLE,
                                                                                              Localisation.LocalisationKeys.SaveMenu.LOAD_TITLE,
                                                                                              Localisation.LocalisationKeys.SaveMenu.NEW_SAVE,
                                                                                              Localisation.LocalisationKeys.SaveMenu.OVERWRITE_QUESTION,
                                                                                              Localisation.LocalisationKeys.SaveMenu.DELETE_QUESTION,
                                                                                              Localisation.LocalisationKeys.SaveMenu.NEWER_VERSION,
                                                                                              Localisation.LocalisationKeys.SaveMenu.DAMAGED,
                                                                                              Test.Localisation.LocalisationKeys.Generic.YES,
                                                                                              Test.Localisation.LocalisationKeys.Generic.NO,
                                                                                              saveSheetNames);

            container.BindSingletonFromInstance(beginMenuLocalisationArgs);
            container.BindSingletonFromInstance(configMenuLocalisationArgs);
            container.BindSingletonFromInstance(languageMenuLocalisationArgs);
            container.BindSingletonFromInstance(partyMenuLocalisationArgs);
            container.BindSingletonFromInstance(saveMenuLocalisationArgs);
        }
    }
}