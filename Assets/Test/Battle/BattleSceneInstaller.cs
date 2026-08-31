using RPGFramework.Battle;
using RPGFramework.Battle.Databases;
using RPGFramework.Battle.SharedTypes;
using RPGFramework.Battle.Loaders;
using RPGFramework.Battle.Providers;
using RPGFramework.DI;
using UnityEngine;

namespace Test.Battle
{
    public class BattleSceneInstaller : SceneInstallerBase
    {
        [SerializeField]
        private BattleAudioProvider m_AudioProvider;

        public override void InstallBindings(IDIContainer container)
        {
            container.BindSingletonFromInstance<IBattleAudioProvider>(m_AudioProvider);

            container.BindSingleton<IBattleArenaDatabase, BattleArenaDatabase>();
            container.BindSingleton<IBattleArenaLoader, BattleArenaLoader>();
            container.BindSingleton<IBattleArenaPresentation, BattleArenaPresentation>();
            container.BindSingleton<IBattleModule, BattleModule>();
        }
    }
}