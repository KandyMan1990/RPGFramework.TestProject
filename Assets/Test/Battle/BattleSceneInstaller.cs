using RPGFramework.Battle;
using RPGFramework.Battle.Databases;
using RPGFramework.Battle.SharedTypes;
using RPGFramework.Battle.Providers;
using RPGFramework.DI;
using UnityEngine;

namespace Test.Battle
{
    internal class BattleSceneInstaller : SceneInstallerBase
    {
        [SerializeField]
        private BattleAudioProvider m_AudioProvider;

        [SerializeField]
        private BattleEncounterDatabase m_Encounters;

        public override void InstallBindings(IDIContainer container)
        {
            container.BindSingletonFromInstance<IBattleAudioProvider>(m_AudioProvider);
            container.BindSingletonFromInstance<IBattleEncounterDatabase>(m_Encounters);

            container.BindSingleton<IBattleArenaDatabase, BattleArenaDatabase>();
        }
    }
}
