using System;
using RPGFramework.Battle.SharedTypes;
using RPGFramework.Battle.SharedTypes.Constants;
using RPGFramework.Core.Databases;
using RPGFramework.Field.SharedTypes;
using RPGFramework.Field.SharedTypes.Constants;
using RPGFramework.Menu.SharedTypes;
using RPGFramework.Menu.SharedTypes.Constants;
using Test.SplashScreen;
using Test.SplashScreen.Constants;

namespace Test
{
    public sealed class ModuleDatabase : IModuleDatabase
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
                       _                               => throw new InvalidOperationException($"{nameof(IModuleDatabase)}::{nameof(IModuleDatabase.GetModuleType)} Unknown module id {moduleId}")
                   };
        }
    }
}