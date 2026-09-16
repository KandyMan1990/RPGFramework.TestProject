using System.Threading.Tasks;
using RPGFramework.Audio;
using RPGFramework.Audio.Music;
using RPGFramework.Audio.Sfx;
using RPGFramework.Battle.SharedTypes.Providers;
using RPGFramework.Core;
using RPGFramework.Core.Audio;
using RPGFramework.Core.Databases;
using RPGFramework.Core.Dialogue.UI;
using RPGFramework.Core.Rendering;
using RPGFramework.Core.SaveData;
using RPGFramework.DI;
using RPGFramework.Field;
using RPGFramework.Field.SharedTypes.Providers;
using RPGFramework.Localisation;
using RPGFramework.Menu.SharedTypes.Providers;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.Universal;

namespace Test
{
    public class TestGlobalInstaller : GlobalInstallerBase
    {
        [SerializeField]
        private MusicAssetProvider m_MusicProvider;

        [SerializeField]
        private AudioMixerGroup[] m_MusicMixerGroups;

        [SerializeField]
        private SfxAssetProvider m_SfxProvider;

        [SerializeField]
        private AudioMixerGroup[] m_SfxMixerGroups;

        [SerializeField]
        private DialogueWindowUiProvider m_DialogueWindowUiProvider;

        [SerializeField]
        private MemoryServiceArgs m_MemoryServiceArgs;

        [SerializeField]
        private UniversalRendererData m_UniversalRendererData;

        [SerializeField]
        private ScreenFadeServiceConfig m_ScreenFadeServiceConfig;

        public override void InstallBindings(IDIContainer container)
        {
            container.BindSingleton<ILocalisationService, LocalisationService>().AsNonLazy();

            ISfxPlayer sfxPlayer = new UnitySfxPlayer();
            sfxPlayer.SetSfxAssetProvider(m_SfxProvider);
            sfxPlayer.SetStemMixerGroups(m_SfxMixerGroups);
            container.BindSingletonFromInstance(sfxPlayer);

            IMusicPlayer musicPlayer = new UnityMusicPlayer();
            musicPlayer.SetMusicAssetProvider(m_MusicProvider);
            musicPlayer.SetStemMixerGroups(m_MusicMixerGroups);
            container.BindSingletonFromInstance(musicPlayer);

            IRendererDataProvider rendererDataProvider = new RendererDataProvider(m_UniversalRendererData);
            container.BindSingletonFromInstance(rendererDataProvider);

            container.BindSingletonFromInstance<IScreenFadeServiceConfig>(m_ScreenFadeServiceConfig);

            container.BindSingleton<IBattleArgsProvider, BattleArgsProvider>();
            container.BindSingleton<IFieldArgsProvider, FieldArgsProvider>();
            container.BindSingleton<IMenuArgsProvider, MenuArgsProvider>();

            container.BindSingletonFromInstance<IAudioIntentPlayer>(new GameAudioIntentPlayer(sfxPlayer, GameAudioIntentMaps.Default));

            container.BindSingleton<ISaveFactory, SaveFactory>();
            container.BindSingleton<IModuleDatabase, ModuleDatabase>();
            container.BindSingleton<IFieldResumeDataStore, FieldResumeDataStore>();

            container.BindSingletonFromInstance<IDialogueWindowUiProvider>(m_DialogueWindowUiProvider);

            container.BindSingletonFromInstance<IMemoryServiceArgs>(m_MemoryServiceArgs);
            container.BindSingletonFromInstance<ITempMemoryArgs>(m_MemoryServiceArgs);

            container.BindSingleton<IBattleCompleteStateProvider, BattleCompleteStateProvider>();

            container.BindSingleton<ISceneDatabase, SceneDatabase>();
        }

        public override Task Bootstrap(IDIResolver resolver)
        {
            IScreenFadeService screenFadeService = resolver.Resolve<IScreenFadeService>();
            screenFadeService.SetFadeToSimple();

            ILocalisationService localisationService = resolver.Resolve<ILocalisationService>();
            return localisationService.InitialiseAsync();
        }
    }
}