using RPGFramework.Core.Data;
using RPGFramework.Core.SaveData;
using RPGFramework.Core.Settings;
using RPGFramework.Localisation;

namespace Test
{
    internal class SettingsFactory : ISettingsFactory
    {
        private readonly ILocalisationService m_LocalisationService;

        public SettingsFactory(ILocalisationService localisationService)
        {
            m_LocalisationService = localisationService;
        }

        void ISettingsFactory.CreateDefaultSettings(ISettingsService settingsService)
        {
            ConfigData_V1 configData = new ConfigData_V1
                                       {
                                           MusicVolume        = 1f,
                                           SfxVolume          = 1f,
                                           BattleMessageSpeed = 0.5f,
                                           FieldMessageSpeed  = 0.5f
                                       };

            configData.SetLanguage(m_LocalisationService.CurrentLanguage);

            settingsService.SetSection(FrameworkSettingsSectionDatabase.CONFIG_DATA, new SaveSection<ConfigData_V1>(Versions.GLOBAL_CONFIG, configData));
        }
    }
}
