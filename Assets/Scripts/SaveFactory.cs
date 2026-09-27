using RPGFramework.Core.SaveData;
using Test.Localisation;

namespace Test
{
    internal class SaveFactory : ISaveFactory
    {
        void ISaveFactory.CreateDefaultSave(ISaveDataService saveDataService)
        {
            SaveSection<TestSaveFileSection> testSaveFileSection = GetDefaultTestSaveFileSection();
            saveDataService.SetSection(GameSaveSectionDatabase.TEST_SAVE_FILE_SECTION, testSaveFileSection);
        }

        void ISaveFactory.OnSaveLoaded(ISaveDataService saveDataService)
        {
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
    }
}