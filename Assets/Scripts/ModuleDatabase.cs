using System;
using RPGFramework.Battle.SharedTypes;
using RPGFramework.Battle.SharedTypes.Constants;
using RPGFramework.Core.Databases;
#if ENABLE_RPG_FRAMEWORK_DEBUG
using RPGFramework.DebugRoom.SharedTypes;
using RPGFramework.DebugRoom.SharedTypes.Constants;
#endif
using RPGFramework.Field.SharedTypes;
using RPGFramework.Field.SharedTypes.Constants;
using RPGFramework.Menu.SharedTypes;
using RPGFramework.Menu.SharedTypes.Constants;
using Test.SplashScreen;
using Test.SplashScreen.Constants;

namespace Test
{
    internal sealed class ModuleDatabase : IModuleDatabase
    {
        Type IModuleDatabase.GetModuleType(byte moduleId)
        {
            return moduleId switch
                   {
                       FieldConstants.MODULE_ID        => typeof(IFieldModule),
                       MenuConstants.MODULE_ID         => typeof(IMenuModule),
                       BattleConstants.MODULE_ID       => typeof(IBattleModule),
                       //ModuleID.World => typeof(IWorldModule),
                       SplashScreenConstants.MODULE_ID => typeof(ISplashScreenModule),
#if ENABLE_RPG_FRAMEWORK_DEBUG
                       DebugRoomConstants.MODULE_ID    => typeof(IDebugRoomModule),
#endif
                       _                               => throw new InvalidOperationException($"{nameof(IModuleDatabase)}::{nameof(IModuleDatabase.GetModuleType)} Unknown module id {moduleId}")
                   };
        }
    }
}