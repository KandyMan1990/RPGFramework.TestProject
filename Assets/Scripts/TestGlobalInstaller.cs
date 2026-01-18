using RPGFramework.Audio;
using RPGFramework.Audio.Music;
using RPGFramework.Audio.Sfx;
using RPGFramework.Core;
using RPGFramework.Core.Audio;
using RPGFramework.Core.Input;
using RPGFramework.Core.SaveDataService;
using RPGFramework.DI;
using RPGFramework.Field;
using RPGFramework.Field.SharedTypes;
using RPGFramework.Localisation;
using RPGFramework.Menu;
using RPGFramework.Menu.SharedTypes;
using UnityEngine;
using UnityEngine.Audio;

namespace Test
{
    public class TestGlobalInstaller : GlobalInstallerBase
    {
        [SerializeField]
        private SfxAssetProvider m_SfxProvider;
        [SerializeField]
        private AudioMixerGroup[] m_SfxMixerGroups;

        public override void InstallBindings(IDIContainer container)
        {
            container.BindSingleton<ILocalisationService, LocalisationService>().AsNonLazy();

            ISfxPlayer sfxPlayer = new UnitySfxPlayer();
            sfxPlayer.SetSfxAssetProvider(m_SfxProvider);
            sfxPlayer.SetStemMixerGroups(m_SfxMixerGroups);
            container.BindSingletonFromInstance(sfxPlayer);

            container.BindSingleton<IMusicPlayer, UnityMusicPlayer>();
            container.BindSingleton<IMenuTypeProvider, MenuTypeProvider>();
            container.BindSingleton<IMenuModule, MenuModule>();
            container.BindSingleton<IFieldModule, FieldModule>();
            container.BindSingletonFromInstance<IAudioIntentPlayer>(new GameAudioIntentPlayer(sfxPlayer, GameAudioIntentMaps.Default));

            container.BindSingleton<IInputRouter, InputRouter>();

            container.BindSingleton<ISaveDataService, SaveDataService>();
            container.BindSingleton<ISaveFactory, SaveFactory>();
            container.BindSingleton<IModuleResumeMap, ModuleResumeMap>();
        }
    }
}