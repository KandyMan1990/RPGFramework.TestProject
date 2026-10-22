using System;
using RPGFramework.Battle.SharedTypes;
using RPGFramework.Core.Databases;
#if ENABLE_RPG_FRAMEWORK_DEBUG
using RPGFramework.DebugRoom.SharedTypes;
#endif
using RPGFramework.Field.SharedTypes;
using RPGFramework.Menu.SharedTypes;
using Test.SplashScreen;

namespace Test
{
    internal sealed class SceneDatabase : ISceneDatabase
    {
        private readonly ISceneDatabase m_This;

        public SceneDatabase()
        {
            m_This = this;
        }

        string ISceneDatabase.GetSceneNameForModule<T>()
        {
            return m_This.GetSceneNameForModule(typeof(T));
        }

        string ISceneDatabase.GetSceneNameForModule(Type type)
        {
            return type switch
                   {
                       Type when type == typeof(IBattleModule)       => "BattleModule",
                       Type when type == typeof(IFieldModule)        => "FieldModule",
                       Type when type == typeof(IMenuModule)         => "MenuModule",
                       Type when type == typeof(ISplashScreenModule) => "SplashScreenModule",
#if ENABLE_RPG_FRAMEWORK_DEBUG
                       Type when type == typeof(IDebugRoomModule)    => "DebugRoomModule",
#endif
                       _                                             => throw new ArgumentOutOfRangeException()
                   };
        }
    }
}