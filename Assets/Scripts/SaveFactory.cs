using RPGFramework.Core.Data;
using RPGFramework.Core.SaveData;
using RPGFramework.Core.Store;
using RPGFramework.Field.SharedTypes;
using RPGFramework.Field.SharedTypes.Constants;
using RPGFramework.Field.SharedTypes.Providers;
using RPGFramework.Hashing;
using RPGFramework.Localisation;
using Test.Localisation;

namespace Test
{
    public class SaveFactory : ISaveFactory
    {
        private const string STARTING_FIELD = "TestField0";

        private readonly ILocalisationService m_LocalisationService;
        private readonly IChangeModuleStore   m_ChangeModuleStore;
        private readonly IFieldArgsProvider   m_FieldArgsProvider;

        public SaveFactory(ILocalisationService localisationService,
                           IChangeModuleStore   changeModuleStore,
                           IFieldArgsProvider   fieldArgsProvider)
        {
            m_LocalisationService = localisationService;
            m_ChangeModuleStore   = changeModuleStore;
            m_FieldArgsProvider   = fieldArgsProvider;
        }

        void ISaveFactory.CreateDefaultSave(ISaveDataService saveDataService)
        {
            m_ChangeModuleStore.SetModuleId(FieldConstants.MODULE_ID);

            SaveSection<TestSaveFileSection> testSaveFileSection = GetDefaultTestSaveFileSection();
            saveDataService.SetSection(GameSaveSectionDatabase.TEST_SAVE_FILE_SECTION, testSaveFileSection);

            SaveSection<ConfigData_V1> configDataSection = GetDefaultConfigDataSection();
            saveDataService.SetSection(FrameworkSaveSectionDatabase.CONFIG_DATA, configDataSection);

            // first map a new game should start on
            FieldArgs fieldArgs = new FieldArgs(Fnv1a64.Hash(STARTING_FIELD), 0);

            m_FieldArgsProvider.Set(fieldArgs);
        }

        void ISaveFactory.OnSaveLoaded(ISaveDataService saveDataService)
        {
            m_ChangeModuleStore.SetModuleId(FieldConstants.MODULE_ID);

            // TODO: the field and spawn the save was written at are not persisted yet, so a loaded game
            // resumes at the start. They belong in the persistent memory bank as declared variables.
            FieldArgs fieldArgs = new FieldArgs(Fnv1a64.Hash(STARTING_FIELD), 0);

            m_FieldArgsProvider.Set(fieldArgs);
        }

        private static SaveSection<TestSaveFileSection> GetDefaultTestSaveFileSection()
        {
            TestSaveFileSection section = new TestSaveFileSection
                                          {
                                              PlayerExperience = 0,
                                              LastWrittenTicks = 0,
                                              TimePlayed       = 0
                                          };

            section.SetPlayerNameLocKey(LocalisationKeys.Generic.PLAYERNAME);
            section.SetCurrentLocationLocKey("Generic/Seaburn");

            return new SaveSection<TestSaveFileSection>(1, section);
        }

        private SaveSection<ConfigData_V1> GetDefaultConfigDataSection()
        {
            ConfigData_V1 section = new ConfigData_V1
                                    {
                                        MusicVolume        = 1f,
                                        SfxVolume          = 1f,
                                        BattleMessageSpeed = 0.5f,
                                        FieldMessageSpeed  = 0.5f
                                    };

            section.SetLanguage(m_LocalisationService.CurrentLanguage);

            return new SaveSection<ConfigData_V1>(1, section);
        }
    }
}