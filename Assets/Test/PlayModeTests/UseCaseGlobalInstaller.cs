using System.Threading.Tasks;
using RPGFramework.Core.SharedTypes;
using RPGFramework.DI;

namespace Test.PlayModeTests
{
    /// <summary>
    /// The game's own global installer with the router swapped for the tests', which adds one route to the game's; keeps
    /// the resolver for the tests to read the game's services through.
    /// </summary>
    internal sealed class UseCaseGlobalInstaller : GlobalInstallerBase
    {
        private GlobalInstallerBase m_Game;
        private IDIResolver         m_Resolver;

        internal IDIResolver Resolver => m_Resolver;

        internal static UseCaseGlobalInstaller Create(GlobalInstallerBase game)
        {
            UseCaseGlobalInstaller installer = CreateInstance<UseCaseGlobalInstaller>();

            installer.m_Game = game;

            return installer;
        }

        public override void InstallBindings(IDIContainer container)
        {
            m_Game.InstallBindings(container);

            container.Unbind<IModuleRouter>();
            container.BindSingleton<IModuleRouter, UseCaseRouter>();
        }

        public override Task Bootstrap(IDIResolver resolver)
        {
            m_Resolver = resolver;

            return m_Game.Bootstrap(resolver);
        }
    }
}
