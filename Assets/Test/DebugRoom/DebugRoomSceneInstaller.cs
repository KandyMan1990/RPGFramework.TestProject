using RPGFramework.Battle.SharedTypes;
using RPGFramework.DebugRoom.SharedTypes;
using RPGFramework.DI;
using RPGFramework.Field;
using RPGFramework.Field.SharedTypes;
using Test.Battle;
using Test.Field;

namespace Test.DebugRoom
{
    internal sealed class DebugRoomSceneInstaller : SceneInstallerBase
    {
        public override void InstallBindings(IDIContainer container)
        {
            container.BindInterfacesToSelfSingleton<FieldDatabase>();
            container.BindSingleton<IDebugSpawnPointLoader, DebugSpawnPointLoader>();
            container.BindSingleton<IDebugBattleArenaDatabase, BattleArenaDatabase>();
        }
    }
}
