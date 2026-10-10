using System.Threading.Tasks;
using RPGFramework.Audio;
using RPGFramework.Audio.Music;
using RPGFramework.Audio.Sfx;
using RPGFramework.Battle.SharedTypes.Stores;
using RPGFramework.Battle.Stores;
using RPGFramework.Core;
using RPGFramework.Core.Audio;
using RPGFramework.Core.Databases;
using RPGFramework.Core.Dialogue.UI;
using RPGFramework.Core.Memory;
using RPGFramework.Core.Rendering;
using RPGFramework.Core.Settings;
using RPGFramework.DI;
using RPGFramework.Field;
using RPGFramework.Field.SharedTypes.Stores;
using RPGFramework.Localisation;
using RPGFramework.Menu;
using RPGFramework.Menu.SharedTypes.Stores;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.Universal;

namespace Test
{
    internal class TestGlobalInstaller : GlobalInstallerBase
    {
        [SerializeField]
        [Tooltip("Play music and sound from the bundles each provider's Build bundles writes, rather than from the providers. Build them first")]
        private bool m_AudioFromBundles;

        [SerializeField]
        private MusicAssetProvider m_MusicProvider;

        [SerializeField]
        private AudioMixerGroup[] m_MusicMixerGroups;

        [SerializeField]
        private SfxAssetProvider m_SfxProvider;

        [SerializeField]
        private AudioMixerGroup[] m_SfxMixerGroups;

        [SerializeField]
        private DialogueWindowUIProvider m_DialogueWindowUIProvider;

        [SerializeField]
        private VariableMapAsset m_VariableMap;

        [SerializeField]
        private UniversalRendererData m_UniversalRendererData;

        [SerializeField]
        private ScreenFadeServiceConfig m_ScreenFadeServiceConfig;

        public override void InstallBindings(IDIContainer container)
        {
            container.BindSingleton<ILocalisationService, LocalisationService>().AsNonLazy();

            ISfxPlayer sfxPlayer = new UnitySfxPlayer();
            sfxPlayer.SetSfxAssetProvider(m_AudioFromBundles ? new BundledSfxAssetProvider() : m_SfxProvider);
            sfxPlayer.SetStemMixerGroups(m_SfxMixerGroups);
            container.BindSingletonFromInstance(sfxPlayer);

            IMusicPlayer musicPlayer = new UnityMusicPlayer();
            musicPlayer.SetMusicAssetProvider(m_AudioFromBundles ? new BundledMusicAssetProvider() : m_MusicProvider);
            musicPlayer.SetStemMixerGroups(m_MusicMixerGroups);
            container.BindSingletonFromInstance(musicPlayer);

            IRendererDataProvider rendererDataProvider = new RendererDataProvider(m_UniversalRendererData);
            container.BindSingletonFromInstance(rendererDataProvider);

            container.BindSingletonFromInstance<IScreenFadeServiceConfig>(m_ScreenFadeServiceConfig);

            container.BindSingleton<IBattleArgsStore, BattleArgsStore>();
            container.BindSingleton<IFieldArgsStore, VariableFieldArgsStore>();
            container.BindSingleton<IMenuArgsStore, MenuArgsStore>();

            container.BindSingletonFromInstance<IAudioIntentPlayer>(new GameAudioIntentPlayer(sfxPlayer, GameAudioIntentMaps.Default));

            container.BindSingleton<IDefaultSettings, DefaultSettings>();
            container.BindSingleton<IModuleDatabase, ModuleDatabase>();
            container.BindSingleton<IFieldResumeDataStore, FieldResumeDataStore>();

            container.BindSingletonFromInstance<IDialogueWindowUIProvider>(m_DialogueWindowUIProvider);

            container.BindSingletonFromInstance<IMemoryServiceArgs>(m_VariableMap);
            container.BindSingletonFromInstance<ITempMemoryArgs>(m_VariableMap);
            container.BindSingletonFromInstance<IVariableMap>(m_VariableMap);

            container.BindSingleton<IBattleCompleteStateStore, BattleCompleteStateStore>();

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