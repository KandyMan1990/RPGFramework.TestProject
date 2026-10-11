using RPGFramework.Core.SharedTypes;
using RPGFramework.Core.Store;
using RPGFramework.Menu.SharedTypes.Constants;
using RPGFramework.ModuleRouter;
using Test.SplashScreen.Constants;

namespace Test
{
    /// <summary>The framework's routes, and the splash screen's: on to the title menu.</summary>
    internal sealed class TestModuleRouter : DefaultModuleRouter
    {
        public TestModuleRouter(ICurrentModuleStore currentModuleStore) : base(currentModuleStore)
        {
        }

        protected override ModuleChange Route(byte moduleId, byte outcome)
        {
            if (moduleId == SplashScreenConstants.MODULE_ID && outcome == SplashScreenConstants.OUTCOME_FINISHED)
            {
                ModuleChange toTitle = ModuleChange.Clear(MenuConstants.MODULE_ID);

                return toTitle;
            }

            ModuleChange change = base.Route(moduleId, outcome);

            return change;
        }
    }
}