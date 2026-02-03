using RPGFramework.Core.Data;
using RPGFramework.Core.SaveDataService;
using RPGFramework.Core.SharedTypes;
using RPGFramework.Localisation;
using Test.Localisation;

namespace Test
{
    public class SaveFactory : ISaveFactory
    {
        private readonly ILocalisationService m_LocalisationService;

        public SaveFactory(ILocalisationService localisationService)
        {
            m_LocalisationService = localisationService;
        }

        void ISaveFactory.CreateDefaultSave(ISaveDataService saveDataService)
        {
            SaveSection<TestSaveFileSection> testSaveFileSection = GetDefaultTestSaveFileSection();
            saveDataService.SetSection(GameSaveSectionDatabase.TEST_SAVE_FILE_SECTION, testSaveFileSection);

            SaveSection<ConfigData_V1> configDataSection = GetDefaultConfigDataSection();
            saveDataService.SetSection(FrameworkSaveSectionDatabase.CONFIG_DATA, configDataSection);

            SaveSection<RuntimeResumeData> runtimeResumeData = GetDefaultRuntimeResumeData();
            saveDataService.SetSection(FrameworkSaveSectionDatabase.RESUME_DATA, runtimeResumeData);
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
        
        private static SaveSection<RuntimeResumeData> GetDefaultRuntimeResumeData()
        {
            RuntimeResumeData section = new RuntimeResumeData
                                        {
                                                ModuleId = (byte)ModuleID.Field,
                                                Arg0 = 0,
                                                Arg1 = 0,
                                                Arg2 = 0,
                                                Arg3 = 0
                                        };

            return new SaveSection<RuntimeResumeData>(1, section);
        }
    }
}