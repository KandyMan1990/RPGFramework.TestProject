using RPGFramework.Core.SharedTypes;
using RPGFramework.Menu.SharedTypes.Constants;
using RPGFramework.ModuleRouter;
using Test.SplashScreen.Constants;

namespace Test
{
    /// <summary>The splash screen's route: on to the title menu. The rest are the framework's.</summary>
    internal sealed class TestModuleRouter : IModuleRouter
    {
        private readonly IDefaultModuleRouter m_Default;

        public TestModuleRouter(IDefaultModuleRouter defaultRouter)
        {
            m_Default = defaultRouter;
        }

        ModuleChange IModuleRouter.Route(byte moduleId, byte outcome)
        {
            if (moduleId == SplashScreenConstants.MODULE_ID && outcome == SplashScreenConstants.OUTCOME_FINISHED)
            {
                return ModuleChange.Clear(MenuConstants.MODULE_ID);
            }

            return m_Default.Route(moduleId, outcome);
        }
    }
}
