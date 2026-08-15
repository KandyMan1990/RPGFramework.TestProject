using System;
using RPGFramework.Battle.SharedTypes;
using RPGFramework.Core.Databases;
using RPGFramework.Field.SharedTypes;
using RPGFramework.Menu.SharedTypes;
using Test.SplashScreen;

namespace Test
{
    public sealed class SceneDatabase : ISceneDatabase
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
                       _                                             => throw new ArgumentOutOfRangeException()
                   };
        }
    }
}