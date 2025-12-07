using RPGFramework.Audio;
using RPGFramework.Audio.Music;
using RPGFramework.Audio.Sfx;
using RPGFramework.DI;
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
        [SerializeField]
        private GenericAudioIdProvider m_GenericAudioIdProvider;

        public override void InstallBindings(IDIContainer container)
        {
            container.BindSingleton<ILocalisationService, LocalisationService>();

            ISfxPlayer sfxPlayer = new UnitySfxPlayer();
            sfxPlayer.SetSfxAssetProvider(m_SfxProvider);
            sfxPlayer.SetStemMixerGroups(m_SfxMixerGroups);

            container.BindSingletonFromInstance(sfxPlayer);
            container.BindSingleton<IMusicPlayer, UnityMusicPlayer>();
            container.BindSingleton<IMenuModule, MenuModule>();
            container.BindSingletonFromInstance<IGenericAudioIdProvider>(m_GenericAudioIdProvider);
        }
    }
}