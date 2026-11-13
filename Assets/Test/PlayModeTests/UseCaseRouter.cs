using RPGFramework.Core.SharedTypes;
using RPGFramework.Field.SharedTypes.Constants;
using RPGFramework.ModuleRouter;

namespace Test.PlayModeTests
{
    /// <summary>The game's router, with one route of the tests' own: from whatever is on top to the field.</summary>
    internal sealed class UseCaseRouter : IModuleRouter
    {
        // Clear of every module's outcomes, which are small numbers, and the debug room's 255.
        internal const byte ENTER_FIELD = 254;

        private readonly IModuleRouter m_Game;

        public UseCaseRouter(IDefaultModuleRouter defaultRouter)
        {
            m_Game = new TestModuleRouter(defaultRouter);
        }

        ModuleChange IModuleRouter.Route(byte moduleId, byte outcome)
        {
            return outcome == ENTER_FIELD ? ModuleChange.Clear(FieldConstants.MODULE_ID) : m_Game.Route(moduleId, outcome);
        }
    }
}
