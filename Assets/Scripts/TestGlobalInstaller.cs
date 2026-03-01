using RPGFramework.Audio;
using RPGFramework.Audio.Music;
using RPGFramework.Audio.Sfx;
using RPGFramework.Core;
using RPGFramework.Core.Audio;
using RPGFramework.Core.DialogueWindow;
using RPGFramework.Core.DialogueWindow.UI;
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
        private MusicAssetProvider m_MusicProvider;
        [SerializeField]
        private AudioMixerGroup[] m_MusicMixerGroups;
        [SerializeField]
        private SfxAssetProvider m_SfxProvider;
        [SerializeField]
        private AudioMixerGroup[] m_SfxMixerGroups;
        [SerializeField]
        private DialogueWindowUiProvider m_DialogueWindowUiProvider;

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

            container.BindSingleton<IMenuTypeProvider, MenuTypeProvider>();
            container.BindSingleton<IMenuModule, MenuModule>();
            container.BindSingleton<IFieldModule, FieldModule>();
            container.BindSingletonFromInstance<IAudioIntentPlayer>(new GameAudioIntentPlayer(sfxPlayer, GameAudioIntentMaps.Default));

            container.BindSingleton<IInputRouter, InputRouter>();

            container.BindSingleton<ISaveDataService, SaveDataService>();
            container.BindSingleton<ISaveFactory, SaveFactory>();
            container.BindSingleton<IModuleResumeMap, ModuleResumeMap>();

            container.BindSingleton<IFieldRegistry, TestFieldRegistry>();
            container.BindSingleton<IFieldPresentation, PrefabFieldPresentation>();
            
            container.BindSingletonFromInstance<IDialogueWindowUiProvider>(m_DialogueWindowUiProvider);

            container.BindSingleton<IDialogueWindowWithText, DialogueWindowWithText>();
            container.BindSingleton<IDialogueWindowWithTextUI, DialogueWindowWithTextUI>();
        }
    }

    public class TestFieldRegistry : IFieldRegistry
    {
        FieldDefinition IFieldRegistry.LoadField(string fieldId)
        {
            return new FieldDefinition
                   {
                           PrefabAddress = fieldId
                   };
        }
    }
}